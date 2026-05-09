using SkyRoute_Domain.Entities;

public class BudgetWingsProvider : IFlightProvider
{
    private readonly IFlightRepository _flightRepo;
    public string ProviderName => "BudgetWings";

    public BudgetWingsProvider(IFlightRepository flightRepo)
    {
        _flightRepo = flightRepo;
    }

    public async Task<IEnumerable<FlightSearchResponseDTO>> GetFlightsAsync(FlightSearchRequestDTO requestDTO)
    {

        if (requestDTO.minimumFreeSeats == null)
        {
            requestDTO.minimumFreeSeats = 1;
        }

        var rawFlights = await _flightRepo.SearchSpecificFlightsAsync(
            requestDTO.AirportOriginId,
            requestDTO.AirportDestinationId,
            requestDTO.cabinType,
            requestDTO.TimeDeparture,
            requestDTO.DurationMinutes,
            requestDTO.minimumFreeSeats);

        return rawFlights
            .Where(f => f.ProviderName == ProviderName)
            .Select(f => new FlightSearchResponseDTO
            {
                Id = f.Id,
                FlightNumber = f.FlightNumber,
                ProviderName = this.ProviderName,
                CodeIATAOrigin = f.AirportOrigin.CodeIATA,
                CityOrigin = f.AirportOrigin?.City?.Name ?? "Unknown",
                CountryOrigin = f.AirportOrigin?.City?.Country?.Name ?? "Unknown",
                CodeIATADestination = f.AirportDestination.CodeIATA,
                CityDestination = f.AirportDestination?.City?.Name ?? "Unkown",
                CountryDestination = f.AirportDestination?.City?.Country?.Name ?? "Unkown",
                TimeDeparture = f.TimeDeparture,
                TimeArrival = f.TimeArrival,
                CabinType = f.CabinType,
                DurationMinutes = f.DurationMinutes,
                SeatsTotal = f.SeatsTotal,
                SeatsFree = f.SeatsFree,
                // Rule: 10% Discount, but minimum $29.99
                PricePerPerson = Math.Max(Math.Round(f.BaseFare * 0.90m, 2), 29.99m),
                PriceTotal = (decimal)(Math.Max(Math.Round(f.BaseFare * 0.90m, 2), 29.99m) * requestDTO.minimumFreeSeats)
            });
    }
}