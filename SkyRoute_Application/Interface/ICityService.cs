public interface ICityService
{
    Task<ServiceResponse<IEnumerable<CityDTO>>> GetCities(CityDTO request);
}
