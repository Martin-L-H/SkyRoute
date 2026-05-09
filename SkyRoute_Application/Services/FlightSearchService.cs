using SkyRoute_Domain.Entities;

public class FlightSearchService : IFlightSearchService
{
    private readonly IEnumerable<IFlightProvider> _providers;

    public FlightSearchService(IEnumerable<IFlightProvider> providers)
    {
        _providers = providers;
    }

    public async Task<IEnumerable<FlightSearchResponseDTO>> SearchAsync(FlightSearchRequestDTO request)
    {

        var searchTasks = _providers.Select(provider => provider.GetFlightsAsync(request));

        var resultsFromAllProviders = await Task.WhenAll(searchTasks);

        return resultsFromAllProviders
            .SelectMany(result => result)
            .OrderBy(f => f.TimeDeparture)
            .ToList();
    }
}