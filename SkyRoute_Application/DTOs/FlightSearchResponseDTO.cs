namespace SkyRoute_Domain.Entities
{
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

        public Airport AirportOrigin { get; set; }

        public string CodeIATADestination { get; set; }

        public Airport AirportDestination { get; set; }

        public decimal PricePerPerson { get; set; }

        public decimal PriceTotal { get; set; }

        public decimal Passnegers {  get; set; }

    }
}
