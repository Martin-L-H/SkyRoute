
using SkyRoute_Domain.Entities;

public class FlightSearchRequestDTO
    {
        public CabinType? cabinType {  get; set; }

        public DateTime? TimeDeparture { get; set; }

        public int? AirportOriginId { get; set; }

        //public Airport? AirportOrigin { get; set; }

        public int? AirportDestinationId { get; set; }

        public int? minimumFreeSeats { get; set; }

        public string Provider {  get; set; }

        public int DurationMinutes { get; set; }

        //public Airport? AirportDestination { get; set; }

        //public decimal PricePerPerson { get; set; }

        //public decimal PriceTotal { get; set; }

    }