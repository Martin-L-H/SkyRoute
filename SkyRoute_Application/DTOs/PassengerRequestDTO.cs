namespace SkyRoute_Domain.Entities
{
    public class PassengerRequestDTO
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string DocumentNumber { get; set; }

        public bool IsPassport { get; set; }

    }
}
