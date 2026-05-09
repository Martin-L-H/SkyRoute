using SkyRoute_Domain.Entities;

public interface IFlightRepository
{
    Task<IEnumerable<Flight>> GetFlightsAsync();
    Task<IEnumerable<Flight>> SearchFlightsAsync(int originId, int destId, CabinType? cabinType, DateTime departureDate);
    Task<Flight?> SearchFlightByNumberAsync(string flightNumber);
}