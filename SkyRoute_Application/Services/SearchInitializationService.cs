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
        var cabinTypes = Enum.GetValues(typeof(CabinType))
            .Cast<CabinType>()
            .Select(c => new EnumDisplayResponseDTO
        (
            id : (int)c,
            name : c.ToString()
        ))
        .ToList();
        //Testing with record class DTOs
        SearchInitializationDTO response = new SearchInitializationDTO(airports, cabinTypes, providers);

        return ServiceResponse<SearchInitializationDTO>.BuildSuccess(response);

    }
}