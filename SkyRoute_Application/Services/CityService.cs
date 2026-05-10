public class CityService : ICityService
{
    private readonly ICityRepository _repository;
    public CityService(ICityRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<CityDTO>> GetCities(CityDTO request)
    {

        var cityList = await _repository.GetCitiesAsync(request.CountryName, request.Name);

        return cityList.Select(found => new CityDTO
        {
            Id = found.Id,
            Name = found.Name ?? "Unknown",
            CountryName = found.Country?.Name ?? "Unknown",
            CountryId = found.Country?.Id ?? 0,
        });

    }
}
