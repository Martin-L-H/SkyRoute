using SkyRoute_Domain.Entities;

public class FlightSearchService : IFlightSearchService
{
    private readonly IEnumerable<IFlightProvider> _providers;

    public FlightSearchService(IEnumerable<IFlightProvider> providers)
    {
        _providers = providers;
    }


    public async Task<ServiceResponse<IEnumerable<FlightSearchResponseDTO>>> SearchAsync(FlightSearchRequestDTO request)
    {


        if (request.AirportOriginId == request.AirportDestinationId && (request.AirportOriginId != null && request.AirportDestinationId != null))
        {

            return ServiceResponse<IEnumerable<FlightSearchResponseDTO>>.BuildError("Origin and Destination cannot be the same!");

        }

        var validProviders = GetAvailableProviders();

        if (!validProviders.Contains(request.Provider) && request.Provider != "All")
        {

            return ServiceResponse<IEnumerable<FlightSearchResponseDTO>>.BuildError("This flight provider is not available!");

        }
        
        var providersToRun = _providers;

        if (request.Provider != "All")
        {

            providersToRun = _providers.Where(p => p.Provider == request.Provider);

        }


        var searchTasks = providersToRun.Select(p => p.GetFlightsAsync(request));

        var resultsFromAllProviders = await Task.WhenAll(searchTasks);


        //Is there a simpler way to do this? Something tells me I shouldn't have to re-do the DTO list but I can't figure out how.
        IEnumerable<FlightSearchResponseDTO> flightListDTO = resultsFromAllProviders
            .SelectMany(result => result.Select(f => new FlightSearchResponseDTO(
                f.id,
                f.flightNumber,
                f.timeDeparture,
                f.timeArrival,
                f.baseFare,
                f.cabinType,
                f.providerName,
                f.codeIATAOrigin,
                f.airportOriginName,
                f.airportOriginId,
                f.cityOriginName,
                f.cityOriginId,
                f.countryOriginName,
                f.countryOriginId,
                f.codeIATADestination,
                f.airportDestinationName,
                f.airportDestinationId,
                f.cityDestinationId,
                f.cityDestinationName,
                f.countryDestinationName,
                f.countryDestinationId,
                f.pricePerPerson,
                f.priceTotal,
                f.seatsTotal,
                f.seatsFree,
                f.durationMinutes
                )))
            .OrderBy(f => f.timeDeparture)
            .ToList();


        //IEnumerable<FlightSearchResponseDTO> filteredList = flightListDTO.Where(p => p.pricePerPerson <= request.maximumPricePerPerson);

        return ServiceResponse<IEnumerable<FlightSearchResponseDTO>>.BuildSuccess(flightListDTO);
    }

    public IEnumerable<string> GetAvailableProviders()
    {
        return _providers.Select(p => p.Provider).ToList();

    }
}