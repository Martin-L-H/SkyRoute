using SkyRoute_Domain.Entities;

public interface IAirportRepository
{
    Task<IEnumerable<Airport>> GetAirportsWithDetailsAsync(string? countryName, string? cityName, string? airportName);
    Task<Airport?> GetAirportByIATAWithDetailsAsync(string iata);
}