using SkyRoute_Domain.Entities;

public interface IFlightSearchService
{
    Task<IEnumerable<FlightSearchResponseDTO>> SearchAsync(FlightSearchRequestDTO requestDTO);
}