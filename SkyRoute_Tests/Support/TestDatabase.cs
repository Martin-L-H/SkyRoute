using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SkyRoute_Tests.Support;

/// <summary>Real SQL Server in a throwaway Docker container (requires Docker running).</summary>
internal sealed class TestDatabase : IAsyncDisposable
{
    private readonly MsSqlContainer _container;
    private readonly string _connectionString;

    private TestDatabase(MsSqlContainer container, string connectionString)
    {
        _container = container;
        _connectionString = connectionString;
    }

    public bool UseMigrations => true;

    public static async Task<TestDatabase> StartAsync()
    {
        var container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();
        await container.StartAsync();

        // Default pool size is 100, so ~100 calls are truly in flight; the rest queue for a pooled connection.
        // A long connect timeout lets those waiters queue instead of failing.
        return new TestDatabase(container, container.GetConnectionString() + ";Connect Timeout=600");
    }

    public void Configure(DbContextOptionsBuilder options) => options
    .UseSqlServer(_connectionString)
    .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

    public ValueTask DisposeAsync() => _container.DisposeAsync();
}