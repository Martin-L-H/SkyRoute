using SkyRoute_Domain.Entities;

public class BookingResponseDTO
{
    public required string ReferenceCode { get; set; }
    public DateTime FlightDepartureTime { get; set; }
    public DateTime FlightArrivalTime { get; set; }
    public int DurationMinutes { get; set; }
    public required string Provider { get; set; }
    public CabinType CabinType { get; set; }
    public int PassengerCount { get; set; }
    public required string FlightNumber { get; set; }
    public decimal PriceTotal { get; set; }
    public required string AirportOriginName { get; set; }
    public required string AirportOriginCode { get; set; }
    public required string CityOrigin { get; set; }
    public required string CountryOrigin { get; set; }
    public required string AirportDestinyName { get; set; }
    public required string AirportDestinyCode { get; set; }
    public required string CityDestiny { get; set; }
    public required string CountryDestiny { get; set; }
}