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
        public string AirportOriginName { get; set; }
        public int AirportOriginId { get; set; }
        public string CityOriginName {  get; set; }
    public int CityOriginId { get; set; }
        public string CountryOriginName { get; set; }
    public int CountryOriginId { get; set; }
        public string CodeIATADestination { get; set; }

        //public Airport AirportDestination { get; set; }
        public string AirportDestinationName {  get; set; }
    public int AirportDestinationId { get; set; }
    public int CityDestinationId { get; set; }
        public string CityDestinationName {  get; set; }

        public string CountryDestinationName {  get; set; }
    public int CountryDestinationId { get; set; }

        public decimal PricePerPerson { get; set; }

        public decimal PriceTotal { get; set; }

        public int SeatsTotal { get; set; }

        public int SeatsFree { get; set; }

        public int DurationMinutes { get; set; }

    }