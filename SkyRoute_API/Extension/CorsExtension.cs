public static class CorsExtension
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        
        services.AddCors(options => { options
            .AddPolicy("AllowCredentials", policy => { policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod(); }); });
        /*
        services.AddCors(options => {
            options
            .AddPolicy("AngularDevPolicy", policy => {
                policy.WithOrigins("http://localhost:4200") //Assumed front-end URL
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
        */
        return services;

    }
}