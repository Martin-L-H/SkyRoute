using SkyRoute_Domain.Entities;

public interface IAirportRepository
{
    Task<IEnumerable<Airport>> GetAirportsAsync();
    Task<IEnumerable<Airport>> GetAirportsWithDetailsAsync();
    Task<Airport?> GetAirportByIdWithDetailsAsync(int id);
}