using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkyRoute_Infrastructure.Context;

namespace SkyRoute_Tests.Support;

/// <summary>
/// Builds a DI container the way the API would: AppDbContext on the test database, and every
/// interface declared in SkyRoute_Application mapped (scoped) to its implementation found in
/// SkyRoute_Application or SkyRoute_Infrastructure.
/// </summary>
internal sealed class TestHost : IAsyncDisposable
{
    private readonly TestDatabase _database;

    private TestHost(TestDatabase database, ServiceProvider services)
    {
        _database = database;
        Services = services;
    }

    public ServiceProvider Services { get; }

    public static async Task<TestHost> CreateAsync()
    {
        var database = await TestDatabase.StartAsync();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<AppDbContext>(options => database.Configure(options));

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

        return new TestHost(database, provider);
    }

    public async ValueTask DisposeAsync()
    {
        await Services.DisposeAsync();
        await _database.DisposeAsync();
    }
}