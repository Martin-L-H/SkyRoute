using SkyRoute_Application.Interface;
using SkyRoute_Domain.Entities;

public class SearchInitializationService : ISearchInitializationService
{
    private readonly IAirportSearchService _airportService;
    private readonly IFlightSearchService _flightService;
    public SearchInitializationService(IAirportSearchService airportService, IFlightSearchService flightRepository)
    {
        _airportService = airportService;
        _flightService = flightRepository;
    }



    public async Task<ServiceResponse<SearchInitializationDTO>> GetMetadataAsync()
    {
        
        IEnumerable<AirportSearchResponseDTO> airports = await _airportService.GetAllAirportsWithDetailsAsync();
        IEnumerable<string> providers =  _flightService.GetAvailableProviders();
        var cabinTypes = Enum.GetValues(typeof(CabinType)) //This code is disgusting, but it works.
            .Cast<CabinType>()
            .Select(c => new EnumDisplayDTO
        {
            Id = (int)c,
            Name = c.ToString()
        })
        .ToList(); ;

        SearchInitializationDTO response = new SearchInitializationDTO();
        response.Airports = airports;
        response.Providers = providers;
        response.CabinTypes = cabinTypes;

        return ServiceResponse<SearchInitializationDTO>.BuildSuccess(response);
    }
}