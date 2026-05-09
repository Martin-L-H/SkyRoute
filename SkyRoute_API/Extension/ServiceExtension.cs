public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        
        services.AddScoped<IFlightProvider, GlobalAirProvider>();
        services.AddScoped<IFlightProvider, BudgetWingsProvider>();
        services.AddScoped<IFlightSearchService, FlightSearchService>();
        services.AddScoped<IFlightRepository, FlightRepository>();

        return services;
    }
}