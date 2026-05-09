using SkyRoute_Domain.Entities;

public interface IFlightProvider
{
    string ProviderName { get; }
    Task<IEnumerable<FlightSearchResponseDTO>> GetFlightsAsync(FlightSearchRequestDTO requestDTO);
}