using System.ComponentModel.DataAnnotations;
using SkyRoute_Domain.Entities;

public class FlightSearchRequestDTO
{

    public CabinType? CabinType { get; set; }

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


    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Minimum free seats must be at least 1")]

    public int minimumFreeSeats { get; set; }

    [Required]
    public required string Provider { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Duration must be positive")]
    public int? DurationMinutes { get; set; }

}