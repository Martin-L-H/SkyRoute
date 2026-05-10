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

        var flightListDTO = resultsFromAllProviders
            .SelectMany(result => result)
            .OrderBy(f => f.TimeDeparture)
            .ToList();

        return ServiceResponse<IEnumerable<FlightSearchResponseDTO>>.BuildSuccess(flightListDTO);
    }

    public IEnumerable<string> GetAvailableProviders()
    {
        return _providers.Select(p => p.Provider).ToList();

    }
}