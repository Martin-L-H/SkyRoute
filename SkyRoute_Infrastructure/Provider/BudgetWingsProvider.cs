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

        decimal seatsRequested = (decimal)requestDTO.minimumFreeSeats; //Awful, I know, but because minimum seats is nullable it's required

        var rawFlights = await _flightRepo.SearchSpecificFlightsAsync(
            requestDTO.AirportOriginId,
            requestDTO.AirportDestinationId,
            requestDTO.cabinType,
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
                CityOriginId = f.AirportOriginId,
                CountryOriginName = f.AirportOrigin?.City?.Country?.Name ?? UNKNOWN_NAME,
                CountryOriginId = f.AirportOrigin.City.CountryId,
                AirportDestinationId = f.AirportDestinationId,
                CodeIATADestination = f.AirportDestination?.CodeIATA ?? UNKNOWN_NAME,
                AirportDestinationName = f.AirportDestination?.PublicName ?? UNKNOWN_NAME,
                CityDestinationId = f.AirportDestinationId,
                CityDestinationName = f.AirportDestination?.City?.Name ?? UNKNOWN_NAME,
                CountryDestinationId = f.AirportDestination.City.CountryId,
                CountryDestinationName = f.AirportDestination?.City?.Country?.Name ?? UNKNOWN_NAME,
                TimeDeparture = f.TimeDeparture,
                TimeArrival = f.TimeArrival,
                CabinType = f.CabinType,
                DurationMinutes = f.DurationMinutes,
                SeatsTotal = f.SeatsTotal,
                SeatsFree = f.SeatsFree,
                BaseFare = f.BaseFare,
                PricePerPerson = GetPricingPerPerson(f.BaseFare),
                PriceTotal = GetPricingPerPerson(f.BaseFare) * seatsRequested,
            });
    }

    public decimal GetPricingPerPerson(decimal baseFare)
    {
        //15% overcharge over base price
        return Math.Round(baseFare * 1.15m, 2);
    }
}