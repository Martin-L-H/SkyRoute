namespace SkyRoute_Domain.Entities
{
    public class Passenger
    {

        public int Id { get; set; }

        public int BookingId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email {  get; set; }

        public string DocumentNumber { get; set; }

        public Booking Booking { get; set; } //Unsure if this needs to be here, but its used for the builder of the entity framework

        public bool IsPassport { get; set; }

    }
}
