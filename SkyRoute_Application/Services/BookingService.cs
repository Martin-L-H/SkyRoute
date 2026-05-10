using SkyRoute_Domain.Entities;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IFlightRepository _flightRepo;
    private readonly IEnumerable<IFlightProvider> _providers;

    public BookingService(IBookingRepository bookingRepo, IFlightRepository flightRepo, IEnumerable<IFlightProvider> providers)
    {
        _bookingRepo = bookingRepo;
        _flightRepo = flightRepo;
        _providers = providers.ToList();
    }

    public async Task<BookingResponseDTO?> CreateBookingAsync(BookingRequestDTO requestDTO)
    {

        var provider = _providers.FirstOrDefault(p => p.Provider.Equals(requestDTO.ProviderName, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {

            return null;

        }

        Flight? flightFound = await _flightRepo.GetFlightByIdAsync(requestDTO.FlightId);

        if (flightFound == null || flightFound.ProviderName != requestDTO.ProviderName)
        {

            return null;

        }

        if (flightFound.SeatsFree < requestDTO.PassengerCount)
        {
            return null;
        }

        bool isInternational = false;

        string referenceCode = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        
        decimal finalPricePerPerson = provider.GetPricingPerPerson(flightFound.BaseFare);
        decimal priceTotal = finalPricePerPerson * requestDTO.PassengerCount;

        if (flightFound.AirportOrigin.City.CountryId != flightFound.AirportDestination.City.CountryId)
        {

            isInternational = true;

        }

        Booking booking = new Booking
        {
            ReferenceCode = referenceCode,
            FlightId = flightFound.Id,
            PassengerCount = requestDTO.PassengerCount,
            PriceTotal = priceTotal,
            Provider = requestDTO.ProviderName,
            IsInternational = isInternational,
            FlightDepartureTime = flightFound.TimeDeparture,
            CabinType = flightFound.CabinType,
            Passengers = requestDTO.PassengerList.Select(p => new Passenger{
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                DocumentNumber = p.DocumentNumber,
                IsPassport = p.IsPassport
            }).ToList(),
        };
        //The actual booking process goes here
        Booking result = await _bookingRepo.CreateBookingAsync(booking);

        if (result == null) 
        { 

            return null;

        }
        //Reduces the flight available seats to prevent overbooking
        _flightRepo.FlightRestSeats(result.FlightId, result.PassengerCount);

        //Start building the DTO
        TimeSpan difference = flightFound.TimeArrival - flightFound.TimeDeparture; //Code to get the duration of the flight

        return new BookingResponseDTO
        {
            ReferenceCode = result.ReferenceCode,
            FlightDepartureTime = flightFound.TimeDeparture,
            FlightArrivalTime = flightFound.TimeArrival,
            DurationMinutes = (int)Math.Round(difference.TotalMinutes),
            Provider = result.Provider,
            CabinType = result.CabinType,
            PassengerCount = result.PassengerCount,
            FlightNumber = flightFound.FlightNumber,
            PriceTotal = result.PriceTotal,

            AirportOriginName = flightFound.AirportOrigin.PublicName,
            AirportOriginCode = flightFound.AirportOrigin.CodeIATA,
            CityOrigin = flightFound.AirportOrigin.City.Name,
            CountryOrigin = flightFound.AirportOrigin.City.Country.Name,

            AirportDestinyName = flightFound.AirportDestination.PublicName,
            AirportDestinyCode = flightFound.AirportDestination.CodeIATA,
            CityDestiny = flightFound.AirportDestination.City.Name,
            CountryDestiny = flightFound.AirportDestination.City.Country.Name
        };
    }
}