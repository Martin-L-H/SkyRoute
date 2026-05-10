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

    public async Task<ServiceResponse<BookingResponseDTO>> CreateBookingAsync(BookingRequestDTO request)
    {

        if (request == null || request.PassengerList == null || request.PassengerList.Count == 0)
        {

            return ServiceResponse<BookingResponseDTO>.BuildError("Data error mismatch");

        }

        if (request.PassengerCount != request.PassengerList.Count)
        {
            return ServiceResponse<BookingResponseDTO>.BuildError("Passenger count mismatch!");
        }

        var provider = _providers.FirstOrDefault(p => p.Provider.Equals(request.ProviderName, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {

            return ServiceResponse<BookingResponseDTO>.BuildError("The provider requested does not exist!");

        }

        Flight? flightFound = await _flightRepo.GetFlightByIdAsync(request.FlightId);

        if (flightFound == null || flightFound.ProviderName != request.ProviderName)
        {

            return ServiceResponse<BookingResponseDTO>.BuildError("Flight provider integrity mismatch!");

        }

        if (flightFound.SeatsFree < request.PassengerCount)
        {

            return ServiceResponse<BookingResponseDTO>.BuildError("There are not enough free seats in this plane!");

        }

        flightFound.SeatsFree = flightFound.SeatsFree - request.PassengerCount;

        bool isInternational = flightFound.AirportOrigin.City.CountryId != flightFound.AirportDestination.City.CountryId;

        if (isInternational && request.PassengerList.Any(p => p.IsPassport != true))
        {
            return ServiceResponse<BookingResponseDTO>.BuildError("All passengers in an international flight are required to have a valid passport!");
        }

        string referenceCode = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        
        decimal finalPricePerPerson = provider.GetPricingPerPerson(flightFound.BaseFare);
        decimal priceTotal = finalPricePerPerson * request.PassengerCount;

        Booking booking = new Booking
        {
            ReferenceCode = referenceCode,
            FlightId = flightFound.Id,
            PassengerCount = request.PassengerCount,
            PriceTotal = priceTotal,
            Provider = request.ProviderName,
            IsInternational = isInternational,
            FlightDepartureTime = flightFound.TimeDeparture,
            CabinType = flightFound.CabinType,
            Passengers = request.PassengerList.Select(p => new Passenger{
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                DocumentNumber = p.DocumentNumber,
                IsPassport = p.IsPassport
            }).ToList(),
        };

        //The actual booking process goes here
        Booking? result = await _bookingRepo.CreateBookingAsync(booking);

        if (result == null) 
        {

            return ServiceResponse<BookingResponseDTO>.BuildError("We were unable to register the booking!");

        }

        //Start building the DTO
        TimeSpan difference = flightFound.TimeArrival - flightFound.TimeDeparture; //Code to get the duration of the flight

        BookingResponseDTO successBookingDTO = new BookingResponseDTO()
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

        return ServiceResponse<BookingResponseDTO>.BuildSuccess(successBookingDTO);
    }
}