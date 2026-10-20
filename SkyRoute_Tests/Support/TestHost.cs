using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SkyRoute_Infrastructure.Context;

namespace SkyRoute_Tests.Support;

/// <summary>
/// Builds a DI container the way the API would: AppDbContext (via a context factory) on the test database, and every
/// interface declared in SkyRoute_Application mapped (scoped) to its implementation found in
/// SkyRoute_Application or SkyRoute_Infrastructure.
/// </summary>
internal sealed class TestHost : IAsyncDisposable
{
    private readonly TestDatabase _database;

    private TestHost(TestDatabase database, ServiceProvider services, BookingLogCounter logs)
    {
        _database = database;
        Logs = logs;
        Services = services;
    }

    public ServiceProvider Services { get; }

    /// <summary>Counts the repository's categorised log events (no seats / deadlock / other exception).</summary>
    public BookingLogCounter Logs { get; }

    public static async Task<TestHost> CreateAsync()
    {
        var database = await TestDatabase.StartAsync();

        var services = new ServiceCollection();
        var logs = new BookingLogCounter();
        services.AddLogging(builder => builder.AddProvider(logs));
        // Your repositories take IDbContextFactory<AppDbContext>, so register the factory. A plain scoped
        // AppDbContext is added too (only if absent) for anything that injects the context directly.
        services.AddDbContextFactory<AppDbContext>(options => database.Configure(options));
        services.TryAddScoped<AppDbContext>(sp =>
            sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

        var applicationAssembly = Assembly.Load("SkyRoute_Application");
        var infrastructureAssembly = typeof(AppDbContext).Assembly;

        foreach (var assembly in new[] { applicationAssembly, infrastructureAssembly })
        {
            var implementations = assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false, IsNested: false });

            foreach (var implementation in implementations)
            {
                foreach (var contract in implementation.GetInterfaces().Where(i => i.Assembly == applicationAssembly))
                {
                    services.AddScoped(contract, implementation);
                }
            }
        }

        var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });

        await using (var scope = provider.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (database.UseMigrations) await context.Database.MigrateAsync();
            else await context.Database.EnsureCreatedAsync();
        }

        return new TestHost(database, provider, logs);
    }

    public async ValueTask DisposeAsync()
    {
        await Services.DisposeAsync();
        await _database.DisposeAsync();
    }
}