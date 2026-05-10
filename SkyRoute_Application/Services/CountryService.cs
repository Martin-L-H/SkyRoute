public class CountryService : ICountryService
{
    private readonly ICountryRepository _repository;
    public CountryService(ICountryRepository repository)
    {
        _repository = repository;
    }
    public async Task<ServiceResponse<IEnumerable<CountryDTO>>> GetCountries(CountryDTO request)
    {

        var countryList = await _repository.GetCountriesAsync(request.Name);

        if (countryList == null || countryList.Count() == 0)
        {
            return ServiceResponse<IEnumerable<CountryDTO>>.BuildError("No countries were found!");
        }

        IEnumerable<CountryDTO> dtoList = countryList.Select(found => new CountryDTO
        {
            Id = found.Id,
            Name = found.Name ?? "Unknown",
        });

        return ServiceResponse<IEnumerable<CountryDTO>>.BuildSuccess(dtoList);

    }
}