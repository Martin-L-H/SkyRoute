using SkyRoute_Domain.Entities;

public class SkyConnectProvider : IFlightProvider
{
    private readonly IFlightRepository _flightRepo;
    private const string PROVIDER_NAME = "SkyConnect";
    private const string UNKNOWN_NAME = "Unknown";
    public string Provider => PROVIDER_NAME;

    public SkyConnectProvider(IFlightRepository flightRepo)
    {
        _flightRepo = flightRepo;
    }

    public async Task<IEnumerable<FlightSearchResponseDTO>> GetFlightsAsync(FlightSearchRequestDTO requestDTO)
    {

        IEnumerable<Flight> rawFlights = await _flightRepo.SearchSpecificFlightsAsync(
            requestDTO.AirportOriginId,
            requestDTO.CityOriginId,
            requestDTO.CountryOriginId,
            requestDTO.AirportDestinationId,
            requestDTO.CityDestinationId,
            requestDTO.CountryDestinationId,
            requestDTO.CabinTypeId,
            requestDTO.TimeDeparture,
            requestDTO.DurationMinutes,
            requestDTO.minimumFreeSeats);

        IEnumerable<FlightSearchResponseDTO> preFilteredDTO = rawFlights
            .Where(f => f.ProviderName == PROVIDER_NAME)
            .Select(f => new FlightSearchResponseDTO
            (
                id: f.Id,
                flightNumber: f.FlightNumber,
                timeDeparture: f.TimeDeparture,
                timeArrival: f.TimeArrival,
                baseFare: f.BaseFare,
                cabinType: f.CabinType,
                providerName: PROVIDER_NAME,
                codeIATAOrigin: f.AirportOrigin?.CodeIATA ?? UNKNOWN_NAME,
                airportOriginName: f.AirportOrigin?.PublicName ?? UNKNOWN_NAME,
                airportOriginId: f.AirportOriginId,
                cityOriginName: f.AirportOrigin?.City?.Name ?? UNKNOWN_NAME,
                cityOriginId: f.AirportOrigin?.City?.Id ?? 0,
                countryOriginName: f.AirportOrigin?.City?.Country?.Name ?? UNKNOWN_NAME,
                countryOriginId: f.AirportOrigin?.City?.CountryId ?? 0,
                codeIATADestination: f.AirportDestination?.CodeIATA ?? UNKNOWN_NAME,
                airportDestinationName: f.AirportDestination?.PublicName ?? UNKNOWN_NAME,
                airportDestinationId: f.AirportDestinationId,
                cityDestinationId: f.AirportDestination?.City?.Id ?? 0,
                cityDestinationName: f.AirportDestination?.City?.Name ?? UNKNOWN_NAME,
                countryDestinationName: f.AirportDestination?.City?.Country?.Name ?? UNKNOWN_NAME,
                countryDestinationId: f.AirportDestination?.City?.CountryId ?? 0,
                pricePerPerson: GetPricingPerPerson(f.BaseFare),
                priceTotal: GetPricingPerPerson(f.BaseFare) * requestDTO.minimumFreeSeats,
                seatsTotal: f.SeatsTotal,
                seatsFree: f.SeatsFree,
                durationMinutes: f.DurationMinutes
            ));
        //Ugly code. The list of flights is obtained then filtered by the maximum allowed price, but because
        //this has to be done after the discount or overcharge calculation, it cannot be made in the repository
        if (requestDTO.maximumPrice != null && requestDTO.maximumPrice > 0)
        {
            preFilteredDTO = preFilteredDTO.Where(x => x.priceTotal <= requestDTO.maximumPrice);
        }
        return preFilteredDTO;
    }

    public decimal GetPricingPerPerson(decimal baseFare)
    {
        //15% overcharge over base price
        return Math.Round(baseFare * 1.15m, 2);
    }
}