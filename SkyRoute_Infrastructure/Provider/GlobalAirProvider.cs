using SkyRoute_Domain.Entities;

public class GlobalAirProvider : IFlightProvider
{
    private readonly IFlightRepository _flightRepo;
    public string ProviderName => "GlobalAir";

    public GlobalAirProvider(IFlightRepository flightRepo)
    {
        _flightRepo = flightRepo;
    }

    public async Task<IEnumerable<FlightSearchResponseDTO>> GetFlightsAsync(FlightSearchRequestDTO requestDTO)
    {


        var rawFlights = await _flightRepo.SearchSpecificFlightsAsync(
            requestDTO.AirportOriginId, 
            requestDTO.AirportDestinationId, 
            requestDTO.cabinType, 
            requestDTO.TimeDeparture, 
            requestDTO.DurationMinutes,
            requestDTO.minimumFreeSeats);

        return rawFlights
            .Where(f => f.ProviderName == ProviderName)
            .Select(f => new FlightSearchResponseDTO
            {
                Id = f.Id,
                FlightNumber = f.FlightNumber,
                ProviderName = this.ProviderName,
                CodeIATAOrigin = f.AirportOrigin.CodeIATA,
                CityOrigin = f.AirportOrigin?.City?.Name ?? "Unknown",
                CountryOrigin = f.AirportOrigin?.City?.Country?.Name ?? "Unknown",
                CodeIATADestination = f.AirportDestination.CodeIATA,
                CityDestination = f.AirportDestination?.City?.Name ?? "Unkown",
                CountryDestination = f.AirportDestination?.City?.Country?.Name ?? "Unkown",
                TimeDeparture = f.TimeDeparture,
                TimeArrival = f.TimeArrival,
                CabinType = f.CabinType,
                DurationMinutes = f.DurationMinutes,
                // Rule: Base + 15%
                PricePerPerson = Math.Round(f.BaseFare * 1.15m, 2),
                PriceTotal = Math.Round((decimal)((f.BaseFare * 1.15m) * requestDTO.minimumFreeSeats), 2)
            });
    }
}