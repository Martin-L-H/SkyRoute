using SkyRoute_Domain.Entities;

public interface ICountryRepository
{
    Task<IEnumerable<Country>> GetCountriesAsync(string? countryName);
}
