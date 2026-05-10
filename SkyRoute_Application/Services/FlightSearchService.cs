public class FlightSearchService : IFlightSearchService
{
    private readonly IEnumerable<IFlightProvider> _providers;

    public FlightSearchService(IEnumerable<IFlightProvider> providers)
    {
        _providers = providers;
    }


    public async Task<IEnumerable<FlightSearchResponseDTO>> SearchAsync(FlightSearchRequestDTO request)
    {
        var providersToRun = _providers;

        if (request.Provider != "All")
        {

            providersToRun = _providers.Where(p => p.Provider == request.Provider);

        }

        var searchTasks = providersToRun.Select(p => p.GetFlightsAsync(request));

        var resultsFromAllProviders = await Task.WhenAll(searchTasks);

        return resultsFromAllProviders
            .SelectMany(result => result)
            .OrderBy(f => f.TimeDeparture)
            .ToList();
    }

    public IEnumerable<string> GetAvailableProviders()
    {
        return _providers.Select(p => p.Provider).ToList();
    }
}