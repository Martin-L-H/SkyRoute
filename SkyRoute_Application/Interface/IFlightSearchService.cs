using SkyRoute_Domain.Entities;

public interface IFlightSearchService
{
    IEnumerable<string> GetAvailableProviders();
    Task<ServiceResponse<IEnumerable<FlightSearchResponseDTO>>> SearchAsync(FlightSearchRequestDTO request);
}