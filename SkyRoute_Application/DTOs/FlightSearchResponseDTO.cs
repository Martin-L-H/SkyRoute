using SkyRoute_Domain.Entities;

public class FlightSearchResponseDTO
{

    public int Id { get; set; }

    public required string FlightNumber { get; set; }

    public DateTime TimeDeparture { get; set; }

    public DateTime TimeArrival { get; set; }

    public decimal BaseFare { get; set; }

    public CabinType CabinType { get; set; }

    public required string ProviderName { get; set; }

    public required string CodeIATAOrigin { get; set; }

    public required string AirportOriginName { get; set; }

    public int AirportOriginId { get; set; }

    public required string CityOriginName { get; set; }

    public int CityOriginId { get; set; }

    public required string CountryOriginName { get; set; }

    public int CountryOriginId { get; set; }

    public required string CodeIATADestination { get; set; }

    public required string AirportDestinationName { get; set; }

    public int AirportDestinationId { get; set; }

    public int CityDestinationId { get; set; }

    public required string CityDestinationName { get; set; }

    public required string CountryDestinationName { get; set; }

    public int CountryDestinationId { get; set; }

    public decimal PricePerPerson { get; set; }

    public decimal PriceTotal { get; set; }

    public int SeatsTotal { get; set; }

    public int SeatsFree { get; set; }

    public int DurationMinutes { get; set; }

}