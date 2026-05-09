namespace SkyRoute_Domain.Entities
{
    public class Booking
    {

        public int Id { get; set; }

        public string ReferenceCode { get; set; }

        public DateTime FlightDepartureTime { get; set; }

        public decimal PriceTotal { get; set; }

        public string Provider {  get; set; }

        public bool IsInternational { get; set; }

        public CabinType CabinType { get; set; }

        public int PassengerCount { get; set; }

        public int FlightId { get; set; }

        //Navigation properties

        public Flight Flight { get; set; }

        public List<Passenger> Passengers { get; set; }

    }
}
