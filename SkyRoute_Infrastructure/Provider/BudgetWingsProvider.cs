public class BudgetWingsProvider : IFlightProvider
{
    private readonly IFlightRepository _flightRepo;
    private const string PROVIDER_NAME = "BudgetWings";
    private const string UNKNOWN_NAME = "Unknown";
    public string Provider => PROVIDER_NAME;

    public BudgetWingsProvider(IFlightRepository flightRepo)
    {
        _flightRepo = flightRepo;
    }

    public async Task<IEnumerable<FlightSearchResponseDTO>> GetFlightsAsync(FlightSearchRequestDTO requestDTO)
    {

        var rawFlights = await _flightRepo.SearchSpecificFlightsAsync(
            requestDTO.AirportOriginId,
            requestDTO.CityOriginId,
            requestDTO.CountryOriginId,
            requestDTO.AirportDestinationId,
            requestDTO.CityDestinationId,
            requestDTO.CountryDestinationId,
            requestDTO.CabinType,
            requestDTO.TimeDeparture,
            requestDTO.DurationMinutes,
            requestDTO.minimumFreeSeats);

        return rawFlights
            .Where(f => f.ProviderName == PROVIDER_NAME)
            .Select(f => new FlightSearchResponseDTO
            {
                Id = f.Id,
                FlightNumber = f.FlightNumber,
                ProviderName = PROVIDER_NAME,
                CodeIATAOrigin = f.AirportOrigin?.CodeIATA ?? UNKNOWN_NAME,
                AirportOriginName = f.AirportOrigin?.PublicName ?? UNKNOWN_NAME,
                AirportOriginId = f.AirportOriginId,
                CityOriginName = f.AirportOrigin?.City?.Name ?? UNKNOWN_NAME,
                CityOriginId = f.AirportOrigin?.City?.Id ?? 0,
                CountryOriginName = f.AirportOrigin?.City?.Country?.Name ?? UNKNOWN_NAME,
                CountryOriginId = f.AirportOrigin?.City?.CountryId ?? 0,
                AirportDestinationId = f.AirportDestinationId,
                CodeIATADestination = f.AirportDestination?.CodeIATA ?? UNKNOWN_NAME,
                AirportDestinationName = f.AirportDestination?.PublicName ?? UNKNOWN_NAME,
                CityDestinationId = f.AirportDestination?.City?.Id ?? 0,
                CityDestinationName = f.AirportDestination?.City?.Name ?? UNKNOWN_NAME,
                CountryDestinationId = f.AirportDestination?.City?.CountryId ?? 0,
                CountryDestinationName = f.AirportDestination?.City?.Country?.Name ?? UNKNOWN_NAME,
                TimeDeparture = f.TimeDeparture,
                TimeArrival = f.TimeArrival,
                CabinType = f.CabinType,
                DurationMinutes = f.DurationMinutes,
                SeatsTotal = f.SeatsTotal,
                SeatsFree = f.SeatsFree,
                BaseFare = f.BaseFare,
                PricePerPerson = GetPricingPerPerson(f.BaseFare),
                PriceTotal = GetPricingPerPerson(f.BaseFare) * requestDTO.minimumFreeSeats,
            });
    }

    public decimal GetPricingPerPerson(decimal baseFare)
    {
        //15% overcharge over base price
        return Math.Round(baseFare * 1.15m, 2);
    }
}