using SkyRoute_Domain.Entities;

public interface IFlightSearchService
{
    IEnumerable<string> GetAvailableProviders();
    Task<IEnumerable<FlightSearchResponseDTO>> SearchAsync(FlightSearchRequestDTO requestDTO);
}