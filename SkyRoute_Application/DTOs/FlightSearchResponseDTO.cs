using SkyRoute_Domain.Entities;

public class FlightSearchResponseDTO
    {

        public int Id { get; set; }

        public string FlightNumber { get; set; }

        public DateTime TimeDeparture { get; set; }

        public DateTime TimeArrival { get; set; }

        public decimal BaseFare { get; set; }

        public CabinType CabinType { get; set; }

        public string ProviderName { get; set; }


        public string CodeIATAOrigin { get; set; }

        //public Airport AirportOrigin { get; set; }
        public string CityOrigin {  get; set; }
        public string CountryOrigin { get; set; }

        public string CodeIATADestination { get; set; }

        //public Airport AirportDestination { get; set; }

        public string CityDestination {  get; set; }

        public string CountryDestination {  get; set; }

        public decimal PricePerPerson { get; set; }

        public decimal PriceTotal { get; set; }

        public int SeatsTotal { get; set; }

        public int SeatsFree { get; set; }

        public int DurationMinutes { get; set; }

    }