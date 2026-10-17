using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

using SkyRoute_Tests.Support;
using Xunit;
using Xunit.Abstractions;

namespace SkyRoute_Tests.Concurrency;

public sealed class BookingRaceConditionTests
{
    private const int DefaultAttempts = 1000000;

    private readonly ITestOutputHelper _output;

    public BookingRaceConditionTests(ITestOutputHelper output) => _output = output;

    /// <summary>Wiring check, so a DI/schema problem is not mistaken for a race failure.</summary>
    [Fact]
    public async Task Host_CanResolveBookingService()
    {
        await using var host = await TestHost.CreateAsync();
        await using var scope = host.Services.CreateAsyncScope();

        Assert.NotNull(scope.ServiceProvider.GetRequiredService<IBookingService>());
    }

    /// <summary>
    /// N callers each park on ONE semaphore. Once all are parked, the gate is released a single
    /// time with Release(N), so they all enter CreateBookingAsync together. Afterwards the
    /// scenario asserts that the database still satisfies the business invariants.
    ///
    /// Override size with SKYROUTE_RACE_COUNT (default 1,000,000) and the give-up time with
    /// SKYROUTE_RACE_TIMEOUT_MIN (default 30). Run only this test with:
    ///   dotnet test --filter "Category=Stress"
    /// </summary>
    [Fact]
    [Trait("Category", "Stress")]
    public async Task CreateBookingAsync_UnderMassiveConcurrency_PreservesInvariants()
    {
        var attempts = ReadInt("SKYROUTE_RACE_COUNT", DefaultAttempts);
        var timeout = TimeSpan.FromMinutes(ReadInt("SKYROUTE_RACE_TIMEOUT_MIN", 30));

        await using var host = await TestHost.CreateAsync();
        var seed = await BookingScenario.SeedAsync(host.Services);

        using var gate = new SemaphoreSlim(0);
        var succeeded = 0;
        var rejected = 0;
        var faulted = 0;
        var faults = new ConcurrentDictionary<string, int>();

        async Task Worker(int index)
        {
            await gate.WaitAsync();
            await Task.Yield(); // never run the service on the thread that called Release()

            try
            {
                var ok = await BookingScenario.InvokeAsync(host.Services, seed, index);
                if (ok) Interlocked.Increment(ref succeeded);
                else Interlocked.Increment(ref rejected);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref faulted);
                faults.AddOrUpdate(Describe(ex), 1, (_, n) => n + 1);
            }
        }

        // Each Worker runs synchronously up to gate.WaitAsync(), so when this loop ends every caller is parked.
        var workers = new Task[attempts];
        for (var i = 0; i < attempts; i++) workers[i] = Worker(i);

        var clock = Stopwatch.StartNew();
        gate.Release(attempts); // <- the starting gun

        var all = Task.WhenAll(workers);
        var first = await Task.WhenAny(all, Task.Delay(timeout));
        clock.Stop();
        Assert.True(first == all, $"Timed out after {timeout}: only {succeeded + rejected + faulted:N0} of {attempts:N0} callers finished.");
        await all;

        var report = new RaceReport(attempts, succeeded, rejected, faulted, faults.ToDictionary(p => p.Key, p => p.Value), clock.Elapsed);
        WriteReport(report);

        Assert.Equal(attempts, report.Succeeded + report.Rejected + report.Faulted);
        var summary = await BookingScenario.AssertInvariantsAsync(host.Services, seed, report);
        _output.WriteLine(summary);
    }

    private void WriteReport(RaceReport r)
    {
        _output.WriteLine($"callers:   {r.Attempts:N0}");
        _output.WriteLine($"succeeded: {r.Succeeded:N0}");
        _output.WriteLine($"rejected:  {r.Rejected:N0}  (service returned a failure)");
        _output.WriteLine($"faulted:   {r.Faulted:N0}  (service threw)");
        _output.WriteLine($"elapsed:   {r.Elapsed}");
        foreach (var pair in r.FaultBreakdown.OrderByDescending(p => p.Value).Take(15))
        {
            _output.WriteLine($"  {pair.Value,10:N0}  {pair.Key}");
        }
    }

    private static string Describe(Exception ex)
    {
        var root = ex.GetBaseException();
        var message = root.Message.Split('\n')[0].Trim();
        if (message.Length > 160) message = message.Substring(0, 160);
        return $"{ex.GetType().Name} -> {root.GetType().Name}: {message}";
    }

    private static int ReadInt(string name, int fallback) =>
        int.TryParse(Environment.GetEnvironmentVariable(name), out var value) && value > 0 ? value : fallback;
}