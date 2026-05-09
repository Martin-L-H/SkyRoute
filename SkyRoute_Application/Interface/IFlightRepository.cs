using SkyRoute_Domain.Entities;

public interface IFlightRepository
{
    Task<IEnumerable<Flight>> SearchSpecificFlightsAsync(int? originId, int? destId, CabinType? cabinType, DateTime? departureDate, int? duration, int? minimumFreeSeats);
    Task<Flight?> GetFlightByIdAsync(int flightId); //Used just for new bookings
}