namespace SkyRoute_Application.Interface
{
    public interface IAirportSearchService
    {
        Task<IEnumerable<AirportSearchResponseDTO>> GetAirportsWithDetailsAsync(string? cityName, string? countryName);

        Task<AirportSearchResponseDTO?> GetAirportByIATAWithDetailsAsync(string iata);
    }
}
