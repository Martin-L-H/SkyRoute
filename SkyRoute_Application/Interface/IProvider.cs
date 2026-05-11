public interface IFlightProvider
{
    string Provider { get; }
    Task<IEnumerable<FlightSearchResponseDTO>> GetFlightsAsync(FlightSearchRequestDTO requestDTO);
    decimal GetPricingPerPerson(decimal baseFare);
}