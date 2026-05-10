public interface ICountryService
{
    Task<ServiceResponse<IEnumerable<CountryDTO>>> GetCountries(CountryDTO request);
}
