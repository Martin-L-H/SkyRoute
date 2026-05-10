using System.ComponentModel.DataAnnotations;

namespace SkyRoute_Domain.Entities
{
    public class PassengerRequestDTO
    {

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string DocumentNumber { get; set; }

        [Required]
        public bool IsPassport { get; set; }

    }
}
