namespace SkyRoute_Application.Interface
{
    public interface IAirportSearchService
    {
        Task<IEnumerable<AirportSearchResponseDTO>> GetAirportsWithDetailsAsync(AirportSearchRequestDTO requestDTO);

        Task<AirportSearchResponseDTO?> GetAirportByIATAWithDetailsAsync(string iata);
    }
}
