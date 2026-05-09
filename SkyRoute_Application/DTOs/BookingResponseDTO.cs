using SkyRoute_Domain.Entities;

public class BookingResponseDTO
{
    public string ReferenceCode { get; set; }
    public DateTime FlightDepartureTime { get; set; }
    public DateTime FlightArrivalTime { get; set; }
    public int DurationMinutes { get; set; }
    public string Provider { get; set; }
    public CabinType CabinType { get; set; }
    public int PassengerCount { get; set; }
    public string FlightNumber { get; set; }
    public decimal PriceTotal { get; set; }
    public string AirportOriginName { get; set; }
    public string AirportOriginCode { get; set; }
    public string CityOrigin { get; set; }
    public string CountryOrigin { get; set; }
    public string AirportDestinyName { get; set; }
    public string AirportDestinyCode { get; set; }
    public string CityDestiny { get; set; }
    public string CountryDestiny { get; set; }
}