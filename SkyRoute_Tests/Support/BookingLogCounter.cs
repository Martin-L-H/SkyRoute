using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace SkyRoute_Tests.Support;

/// <summary>
/// Counts log events by EventId without formatting any message, so it is cheap even under a million calls.
/// The ids match the ones BookingRepository logs with: 1001 no seats, 1002 deadlock victim (SQL error 1205),
/// 1003 any other exception.
/// </summary>
internal sealed class BookingLogCounter : ILoggerProvider
{
    public const int NoSeats = 1001;
    public const int Deadlock = 1002;
    public const int OtherException = 1003;

    private readonly ConcurrentDictionary<int, int> _counts = new();

    public int Count(int eventId) => _counts.TryGetValue(eventId, out var n) ? n : 0;

    public ILogger CreateLogger(string categoryName) => new CounterLogger(_counts);

    public void Dispose() { }

    private sealed class CounterLogger : ILogger
    {
        private readonly ConcurrentDictionary<int, int> _counts;
        public CounterLogger(ConcurrentDictionary<int, int> counts) => _counts = counts;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (eventId.Id is >= BookingLogCounter.NoSeats and <= BookingLogCounter.OtherException)
            {
                _counts.AddOrUpdate(eventId.Id, 1, (_, n) => n + 1);
            }
        }
    }
}