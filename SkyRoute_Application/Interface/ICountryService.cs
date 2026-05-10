public interface ICountryService
{
    Task<IEnumerable<CountryDTO>> GetCountries(CountryDTO request);
}
