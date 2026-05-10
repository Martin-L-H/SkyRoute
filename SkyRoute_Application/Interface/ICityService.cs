public interface ICityService
{
    Task<IEnumerable<CityDTO>> GetCities(CityDTO request);
}
