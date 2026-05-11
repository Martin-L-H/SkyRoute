using System.ComponentModel.DataAnnotations;
using SkyRoute_Domain.Entities;

public class FlightSearchRequestDTO
{

    public CabinType? CabinType { get; set; }

    public DateTime? TimeDeparture { get; set; }

    public int? AirportOriginId { get; set; }

    public int? AirportDestinationId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Minimum free seats must be at least 1")]

    public int minimumFreeSeats { get; set; }

    [Required]
    public string Provider { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Duration must be positive")]
    public int? DurationMinutes { get; set; }

}