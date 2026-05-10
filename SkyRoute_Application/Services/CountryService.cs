public class CountryService : ICountryService
{
    private readonly ICountryRepository _repository;
    public CountryService(ICountryRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<CountryDTO>> GetCountries(CountryDTO request)
    {

        var countryList = await _repository.GetCountriesAsync(request.Name);

        return countryList.Select(found => new CountryDTO
        {
            Id = found.Id,
            Name = found.Name ?? "Unknown",
        });

    }
}