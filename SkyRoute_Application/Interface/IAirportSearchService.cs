namespace SkyRoute_Application.Interface
{
    public interface IAirportSearchService
    {
        Task <IEnumerable<AirportSearchResponseDTO>>GetAllAirportsWithDetailsAsync();
        Task<ServiceResponse<IEnumerable<AirportSearchResponseDTO>>> GetAirportsWithDetailsAsync(AirportSearchRequestDTO request);

        Task<ServiceResponse<AirportSearchResponseDTO>> GetAirportByIATAWithDetailsAsync(AirportSearchRequestDTO request);
    }
}
