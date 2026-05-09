using SkyRoute_Domain.Entities;

public interface IAirportRepository
{
    Task<IEnumerable<Airport>> GetAirportsWithDetailsAsync(string? cityName, string? countryName);
    Task<Airport?> GetAirportByIATAWithDetailsAsync(string iata);
}