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
        var rawFlights = await _flightRepo.SearchFlightsAsync(requestDTO.AirportOriginId, requestDTO.AirportDestinationId, requestDTO.cabinType, requestDTO.TimeDeparture);

        return rawFlights
            .Where(f => f.ProviderName == ProviderName)
            .Select(f => new FlightSearchResponseDTO
            {
                Id = f.Id,
                FlightNumber = f.FlightNumber,
                ProviderName = this.ProviderName,
                CodeIATAOrigin = f.AirportOrigin.CodeIATA,
                CodeIATADestination = f.AirportDestination.CodeIATA,
                TimeDeparture = f.TimeDeparture,
                TimeArrival = f.TimeArrival,
                CabinType = f.CabinType,
                // Rule: 10% Discount for BudgetWings
                PricePerPerson = Math.Round(f.BaseFare * 0.90m, 2),
                PriceTotal = Math.Round((f.BaseFare * 0.90m) * requestDTO.Passengers, 2)
            });
    }
}