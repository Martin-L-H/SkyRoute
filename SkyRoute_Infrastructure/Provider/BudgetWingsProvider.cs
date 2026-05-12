using SkyRoute_Domain.Entities;

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
        /*
        return rawFlights
            .Where(f => f.ProviderName == PROVIDER_NAME)
            .Select(f => new FlightSearchResponseDTO
            (
                id = f.Id,
                flightNumber = f.FlightNumber,
                providerName = PROVIDER_NAME,
                codeIATAOrigin = f.AirportOrigin?.CodeIATA ?? UNKNOWN_NAME,
                airportOriginName = f.AirportOrigin?.PublicName ?? UNKNOWN_NAME,
                airportOriginId = f.AirportOriginId,
                cityOriginName = f.AirportOrigin?.City?.Name ?? UNKNOWN_NAME,
                cityOriginId = f.AirportOrigin?.City?.Id ?? 0,
                countryOriginName = f.AirportOrigin?.City?.Country?.Name ?? UNKNOWN_NAME,
                countryOriginId = f.AirportOrigin?.City?.CountryId ?? 0,
                airportDestinationId = f.AirportDestinationId,
                codeIATADestination = f.AirportDestination?.CodeIATA ?? UNKNOWN_NAME,
                airportDestinationName = f.AirportDestination?.PublicName ?? UNKNOWN_NAME,
                cityDestinationId = f.AirportDestination?.City?.Id ?? 0,
                cityDestinationName = f.AirportDestination?.City?.Name ?? UNKNOWN_NAME,
                countryDestinationId = f.AirportDestination?.City?.CountryId ?? 0,
                countryDestinationName = f.AirportDestination?.City?.Country?.Name ?? UNKNOWN_NAME,
                timeDeparture = f.TimeDeparture,
                timeArrival = f.TimeArrival,
                cabinType = f.CabinType,
                durationMinutes = f.DurationMinutes,
                seatsTotal = f.SeatsTotal,
                seatsFree = f.SeatsFree,
                baseFare = f.BaseFare,
                pricePerPerson = GetPricingPerPerson(f.BaseFare),
                priceTotal = GetPricingPerPerson(f.BaseFare) * requestDTO.minimumFreeSeats
            ));
        */
        return rawFlights
            .Where(f => f.ProviderName == PROVIDER_NAME)
            .Select(f => new FlightSearchResponseDTO
            (
                id : f.Id,
                flightNumber : f.FlightNumber,
                providerName : PROVIDER_NAME,
                codeIATAOrigin : f.AirportOrigin?.CodeIATA ?? UNKNOWN_NAME,
                airportOriginName : f.AirportOrigin?.PublicName ?? UNKNOWN_NAME,
                airportOriginId : f.AirportOriginId,
                cityOriginName : f.AirportOrigin?.City?.Name ?? UNKNOWN_NAME,
                cityOriginId : f.AirportOrigin?.City?.Id ?? 0,
                countryOriginName : f.AirportOrigin?.City?.Country?.Name ?? UNKNOWN_NAME,
                countryOriginId : f.AirportOrigin?.City?.CountryId ?? 0,
                airportDestinationId : f.AirportDestinationId,
                codeIATADestination : f.AirportDestination?.CodeIATA ?? UNKNOWN_NAME,
                airportDestinationName : f.AirportDestination?.PublicName ?? UNKNOWN_NAME,
                cityDestinationId : f.AirportDestination?.City?.Id ?? 0,
                cityDestinationName : f.AirportDestination?.City?.Name ?? UNKNOWN_NAME,
                countryDestinationId : f.AirportDestination?.City?.CountryId ?? 0,
                countryDestinationName : f.AirportDestination?.City?.Country?.Name ?? UNKNOWN_NAME,
                timeDeparture : f.TimeDeparture,
                timeArrival : f.TimeArrival,
                cabinType : f.CabinType,
                durationMinutes : f.DurationMinutes,
                seatsTotal : f.SeatsTotal,
                seatsFree : f.SeatsFree,
                baseFare : f.BaseFare,
                pricePerPerson : GetPricingPerPerson(f.BaseFare),
                priceTotal : GetPricingPerPerson(f.BaseFare) * requestDTO.minimumFreeSeats
            ));

    }

    public decimal GetPricingPerPerson(decimal baseFare)
    {
        //15% overcharge over base price
        return Math.Round(baseFare * 1.15m, 2);
    }
}