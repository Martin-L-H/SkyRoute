using System.ComponentModel.DataAnnotations;
using SkyRoute_Domain.Entities;

public class FlightSearchRequestDTO
{

    public CabinType? CabinTypeId { get; set; }

    public DateTime? TimeDeparture { get; set; }

    public int? AirportOriginId { get; set; }
    public string? AirportOriginName { get; set; }
    public int? CityOriginId { get; set; }
    public string? CityOriginName { get; set; }
    public int? CountryOriginId { get; set; }
    public string? CountryOriginName { get; set; }
    public int? AirportDestinationId { get; set; }
    public string? AirportDestinationName { get; set; }
    public int? CityDestinationId { get; set; }
    public string? CityDestinationName { get; set; }
    public int? CountryDestinationId { get; set; }
    public string? CountryDestinationName { get; set; }

    public decimal? maximumPricePerPerson { get; set; }


    [Required]
    [Range(1, 9, ErrorMessage = "You can only search for a number between 1 and 9")]

    public int minimumFreeSeats { get; set; }

    [Required]
    public required string Provider { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Duration must be positive")]
    public int? DurationMinutes { get; set; }

}