using System.ComponentModel.DataAnnotations;
using SkyRoute_Domain.Entities;

public class BookingRequestDTO
{

    [Required]
    public int FlightId { get; set; }

    [Required]
    public string ProviderName { get; set; }

    [Required]
    public int PassengerCount { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one passenger detail must be provided.")]
    public List<PassengerRequestDTO> PassengerList {  set; get; } 
}