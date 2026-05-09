public static class CorsExtension
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options => { options.AddPolicy("AllowCredentials", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });
        return services;
    }
}