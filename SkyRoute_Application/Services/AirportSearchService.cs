using SkyRoute_Application.Interface;

public class AirportSearchService : IAirportSearchService
{
    private readonly IAirportRepository _repository;
    public AirportSearchService(IAirportRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<AirportSearchResponseDTO>> GetAirportsWithDetailsAsync(string? cityName, string? countryName)
    {

        var airportList = await _repository.GetAirportsWithDetailsAsync(cityName, countryName);

        return airportList.Select(a => new AirportSearchResponseDTO
        {
            Name = a.PublicName,
            CodeIATA = a.CodeIATA,
            CityName = a.City?.Name ?? "Unknown City",
            CountryName = a.City?.Country?.Name ?? "Unknown Country",
            CountryID = a.City?.CountryId ?? 0
        });

    }
    public async Task<AirportSearchResponseDTO?> GetAirportByIATAWithDetailsAsync(string iata)
    {

        var found = await _repository.GetAirportByIATAWithDetailsAsync(iata);

        if (found == null)
        {

            return null;

        }

        AirportSearchResponseDTO airportDTO = new AirportSearchResponseDTO()
        {
            Name = found.PublicName,
            CodeIATA = found.CodeIATA,
            CityName = found.City?.Name ?? "Unknown",
            CountryName = found.City?.Country?.Name ?? "Unknown",
            CountryID = found.City?.CountryId ?? 0
        };
        
        return airportDTO;
    }
}