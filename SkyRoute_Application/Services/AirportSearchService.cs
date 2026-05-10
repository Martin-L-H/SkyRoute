using SkyRoute_Application.Interface;

public class AirportSearchService : IAirportSearchService
{
    private readonly IAirportRepository _repository;
    public AirportSearchService(IAirportRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<AirportSearchResponseDTO>> GetAirportsWithDetailsAsync(AirportSearchRequestDTO requestDTO)
    {

        var airportList = await _repository.GetAirportsWithDetailsAsync(requestDTO.CountryName, requestDTO.CityName, requestDTO.AirportName);

        return airportList.Select(found => new AirportSearchResponseDTO
        {
            Id = found.Id,
            Name = found.PublicName ?? "Unknown",
            CodeIATA = found.CodeIATA ?? "---",
            CityName = found.City?.Name ?? "Unknown City",
            CountryName = found.City?.Country?.Name ?? "Unknown Country",
            CountryID = found.City?.CountryId ?? 0,
            CityId = found.City?.Id ?? 0
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
            Id = found.Id,
            Name = found.PublicName,
            CodeIATA = found.CodeIATA,
            CityName = found.City?.Name ?? "Unknown City",
            CountryName = found.City?.Country?.Name ?? "Unknown Country",
            CountryID = found.City?.CountryId ?? 0,
            CityId = found.City?.Id ?? 0
        };
        
        return airportDTO;
    }
}