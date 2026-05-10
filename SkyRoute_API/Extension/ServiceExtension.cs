using SkyRoute_Application.Interface;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        
        services.AddScoped<IFlightProvider, GlobalAirProvider>();
        services.AddScoped<IFlightProvider, BudgetWingsProvider>();
        services.AddScoped<IFlightSearchService, FlightSearchService>();
        services.AddScoped<IFlightRepository, FlightRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IAirportSearchService, AirportSearchService>();
        services.AddScoped<IAirportRepository, AirportRepository> ();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICountryService, CountryService>();

        return services;
    }
}