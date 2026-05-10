using SkyRoute_Domain.Entities;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetCitiesAsync(string? countryName, string? cityName);
}
