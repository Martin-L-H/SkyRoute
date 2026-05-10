using AspNetCoreRateLimit;

public static class RateLimitingExtension
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.Configure<IpRateLimitOptions>(options => { options.GeneralRules = new List<RateLimitRule> { new RateLimitRule { Endpoint = "*", Period = "5s", Limit = 10, } }; });
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        return services;
    }
}