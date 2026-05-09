namespace SkyRoute_Domain.Entities
{
    public class Flight
    {

        public int Id { get; set; }

        public string FlightNumber { get; set; }
       
        public DateTime TimeDeparture { get; set; }

        public DateTime TimeArrival { get; set; }

        public decimal BaseFare { get; set; }

        public CabinType CabinType { get; set; }

        public string ProviderName { get; set; }


        public int AirportOriginId { get; set; }

        public Airport AirportOrigin { get; set; }

        public int AirportDestinationId { get; set; }

        public Airport AirportDestination { get; set; }

        public int SeatsTotal { get; set; }

        public int SeatsFree { get; set; }

        public int DurationMinutes { get; set; }

    }
}
