using SkyRoute_Application.Interface;

public class AirportSearchService : IAirportSearchService
{
    private readonly IAirportRepository _repository;
    public AirportSearchService(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AirportSearchResponseDTO>> GetAllAirportsWithDetailsAsync()
    {
        var airportList = await _repository.GetAirportsWithDetailsAsync("","","");

        IEnumerable<AirportSearchResponseDTO> dtoList = airportList.Select(found => new AirportSearchResponseDTO
        (
            id : found.Id,
            name : found.PublicName ?? "Unknown",
            codeIATA : found.CodeIATA ?? "---",
            cityName : found.City?.Name ?? "Unknown City",
            cityId: found.City?.Id ?? 0,
            countryName : found.City?.Country?.Name ?? "Unknown Country",
            countryId : found.City?.CountryId ?? 0
        ));

        return dtoList;
    }
    public async Task<ServiceResponse<IEnumerable<AirportSearchResponseDTO>>> GetAirportsWithDetailsAsync(AirportSearchRequestDTO request)
    {

        var airportList = await _repository.GetAirportsWithDetailsAsync(request.CountryName, request.CityName, request.AirportName);

        if (airportList == null || airportList.Count() == 0)
        {
            return ServiceResponse<IEnumerable<AirportSearchResponseDTO>>.BuildError("No airports were found!");
        }

        IEnumerable<AirportSearchResponseDTO> dtoList = airportList.Select(found => new AirportSearchResponseDTO
        (
            id : found.Id,
            name : found.PublicName ?? "Unknown",
            codeIATA : found.CodeIATA ?? "---",
            cityName : found.City?.Name ?? "Unknown City",
            cityId: found.City?.Id ?? 0,
            countryName : found.City?.Country?.Name ?? "Unknown Country",
            countryId : found.City?.CountryId ?? 0
            
        ));

        return ServiceResponse<IEnumerable<AirportSearchResponseDTO>>.BuildSuccess(dtoList);
    }

    public async Task<ServiceResponse<AirportSearchResponseDTO>> GetAirportByIATAWithDetailsAsync(AirportSearchRequestDTO request)
    {

        if (string.IsNullOrWhiteSpace(request.CodeIATA))
        {
            return ServiceResponse<AirportSearchResponseDTO>.BuildError("Invalid IATA code!");
        }

        var found = await _repository.GetAirportByIATAWithDetailsAsync(request.CodeIATA.ToUpper());

        if (found == null)
        {

            return ServiceResponse<AirportSearchResponseDTO>.BuildError("No airport was found with this IATA code!");

        }

        AirportSearchResponseDTO response = new AirportSearchResponseDTO
        (
            id : found.Id,
            name : found.PublicName,
            codeIATA : found.CodeIATA,
            cityName : found.City?.Name ?? "Unknown City",
            cityId : found.City?.Id ?? 0,
            countryName : found.City?.Country?.Name ?? "Unknown Country",
            countryId : found.City?.CountryId ?? 0
        );

        return ServiceResponse<AirportSearchResponseDTO>.BuildSuccess(response);
    }
}