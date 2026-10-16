using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SkyRoute_Tests.Support;

/// <summary>
/// In-RAM SQLite using a named shared-cache database. A keep-alive connection holds the
/// database open; every DbContext opens its own connection to the same in-memory store.
/// NOTE: SQLite allows one writer at a time, so it can serialize (and therefore hide) some
/// races. Use -Database SqlServer / Postgres for a faithful isolation-level test.
/// </summary>
internal sealed class TestDatabase : IAsyncDisposable
{
    private readonly SqliteConnection _keepAlive;
    private readonly string _connectionString;

    private TestDatabase(string connectionString, SqliteConnection keepAlive)
    {
        _connectionString = connectionString;
        _keepAlive = keepAlive;
    }

    public bool UseMigrations => false; // migrations may contain provider-specific SQL; build the schema from the model

    public static Task<TestDatabase> StartAsync()
    {
        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = "skyroute_" + Guid.NewGuid().ToString("N"),
            Mode = SqliteOpenMode.Memory,
            Cache = SqliteCacheMode.Shared,
            DefaultTimeout = 600, // seconds: let lock waiters queue instead of failing after 30s
        }.ToString();

        var keepAlive = new SqliteConnection(connectionString);
        keepAlive.Open();
        return Task.FromResult(new TestDatabase(connectionString, keepAlive));
    }

    public void Configure(DbContextOptionsBuilder options) => options.UseSqlite(_connectionString);

    public async ValueTask DisposeAsync()
    {
        await _keepAlive.DisposeAsync();
        SqliteConnection.ClearAllPools();
    }
}