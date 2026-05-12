using Microsoft.Extensions.Logging;
using SkyRoute_Domain.Entities;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IFlightRepository _flightRepo;
    private readonly IEnumerable<IFlightProvider> _providers;
    private readonly ILogger<BookingService> _logger;
    private const int REFERENCE_CODE_LENGTH = 6;
    public const string UNKNOWN_DATA = "Unknown";

    public BookingService(IBookingRepository bookingRepo, IFlightRepository flightRepo, IEnumerable<IFlightProvider> providers, ILogger<BookingService> logger)
    {
        _bookingRepo = bookingRepo;
        _flightRepo = flightRepo;
        _providers = providers.ToList();
        _logger = logger;
    }

    public async Task<ServiceResponse<BookingResponseDTO>> CreateBookingAsync(BookingRequestDTO request)
    {


        if (request == null || request.PassengerList == null || request.PassengerList.Count == 0)
        {

            _logger.LogError($"Failed to create booking for flight, data error mismatch {request}");

            return ServiceResponse<BookingResponseDTO>.BuildError("Data error mismatch");

        }

        if (request.PassengerCount != request.PassengerList.Count)
        {

            _logger.LogError($"Failed to create booking for flight, passenger count mismatch {request}");

            return ServiceResponse<BookingResponseDTO>.BuildError("Passenger count mismatch!");

        }

        var provider = _providers.FirstOrDefault(p => p.Provider.Equals(request.ProviderName, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {

            _logger.LogError($"Failed to create booking for flight, provider requested does not exist {request}");

            return ServiceResponse<BookingResponseDTO>.BuildError("The provider requested does not exist!");

        }

        Flight? flightFound = await _flightRepo.GetFlightByIdAsync(request.FlightId);

        if (flightFound == null || flightFound.ProviderName != request.ProviderName)
        {

            _logger.LogError($"Failed to create booking for flight, flight provider integrity mismatch {request}");

            return ServiceResponse<BookingResponseDTO>.BuildError("Flight provider integrity mismatch!");

        }

        if (flightFound.SeatsFree < request.PassengerCount)
        {

            _logger.LogError($"Failed to create booking for flight, not enough free seats in this plane {request}");

            return ServiceResponse<BookingResponseDTO>.BuildError("There are not enough free seats in this plane!");

        }
        //Weird check, but copilot insisted that if I didn't put this here the isInternational bool check could throw a null reference exception
        if (flightFound.AirportOrigin?.City?.Country == null || flightFound.AirportDestination?.City?.Country == null)
        {

            _logger.LogError($"Failed to create booking for flight, flight data is incomplete {request}");

            return ServiceResponse<BookingResponseDTO>.BuildError("Flight data is incomplete!");

        }

        bool isInternational = flightFound.AirportOrigin.City.CountryId != flightFound.AirportDestination.City.CountryId;

        if (isInternational && request.PassengerList.Any(p => p.IsPassport != true))
        {

            _logger.LogError($"Failed to create booking for flight, not all passengers in this international flight have passport {request}");

            return ServiceResponse<BookingResponseDTO>.BuildError("All passengers in an international flight are required to have a valid passport!");

        }

        string referenceCode = Guid.NewGuid().ToString("N").Substring(0, REFERENCE_CODE_LENGTH).ToUpper();

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
            Passengers = request.PassengerList.Select(p => new Passenger
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                DocumentNumber = p.DocumentNumber,
                IsPassport = p.IsPassport
            }).ToList(),
        };

        Booking? result = await _bookingRepo.CreateBookingAsync(booking);

        if (result == null)
        {

            _logger.LogError($"booking request valid but returned null from the repository {request}");

            return ServiceResponse<BookingResponseDTO>.BuildError("We were unable to register the booking!");

        }
        //Start building the DTO
        TimeSpan difference = flightFound.TimeArrival - flightFound.TimeDeparture; //Code to get the duration of the flight

        BookingResponseDTO successBookingDTO = new BookingResponseDTO
        (
            referenceCode : result.ReferenceCode,
            flightDepartureTime : flightFound.TimeDeparture,
            flightArrivalTime : flightFound.TimeArrival,
            durationMinutes : (int)Math.Round(difference.TotalMinutes),
            provider : result.Provider,
            cabinType : result.CabinType,
            passengerCount : result.PassengerCount,
            flightNumber : flightFound.FlightNumber,
            priceTotal : result.PriceTotal,

            airportOriginName : flightFound.AirportOrigin?.PublicName ?? UNKNOWN_DATA,
            airportOriginCode : flightFound.AirportOrigin?.CodeIATA ?? UNKNOWN_DATA,
            cityOrigin : flightFound.AirportOrigin?.City?.Name ?? UNKNOWN_DATA,
            countryOrigin : flightFound.AirportOrigin?.City?.Country?.Name ?? UNKNOWN_DATA,

            airportDestinyName : flightFound.AirportDestination?.PublicName ?? UNKNOWN_DATA,
            airportDestinyCode : flightFound.AirportDestination?.CodeIATA ?? UNKNOWN_DATA,
            cityDestiny : flightFound.AirportDestination?.City?.Name ?? UNKNOWN_DATA,
            countryDestiny : flightFound.AirportDestination?.City?.Country?.Name ?? UNKNOWN_DATA
        );

        return ServiceResponse<BookingResponseDTO>.BuildSuccess(successBookingDTO);

    }
}