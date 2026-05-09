using SkyRoute_Domain.Entities;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IFlightRepository _flightRepo;

    public BookingService(IBookingRepository bookingRepo, IFlightRepository flightRepo)
    {
        _bookingRepo = bookingRepo;
        _flightRepo = flightRepo;
    }

    public async Task<BookingResponseDTO?> CreateBookingAsync(BookingRequestDTO requestDTO)
    {
        Flight? flightFound = await _flightRepo.GetFlightByIdAsync(requestDTO.FlightId);

        if (flightFound == null)
        {

            return null;

        }

        if (requestDTO.PassengerList.Count != requestDTO.PassengerCount)
        {
            return null;
        }

        bool isInternational = false;

        string referenceCode = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

        decimal finalPrice = CalculateFinalPrice(flightFound.BaseFare, requestDTO.ProviderName, requestDTO.PassengerCount);

        if (flightFound.AirportOrigin.City.CountryId != flightFound.AirportDestination.City.CountryId)
        {
            isInternational = true;
        }

        Booking booking = new Booking
        {
            ReferenceCode = referenceCode,
            FlightId = flightFound.Id,
            PassengerCount = requestDTO.PassengerCount,
            PriceTotal = finalPrice,
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

        Booking result = await _bookingRepo.CreateBookingAsync(booking);

        if (result == null) 
        { 

            return null;

        }

        //Code to get the duration of the flight
        TimeSpan difference = flightFound.TimeArrival - flightFound.TimeDeparture;

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

    private decimal CalculateFinalPrice(decimal baseFare, string provider, int count)
    {
        decimal pricePerPerson = provider == "BudgetWings"
            ? Math.Max(Math.Round(baseFare * 0.90m, 2), 29.99m)
            : Math.Round(baseFare * 1.15m, 2);

        return pricePerPerson * count;
    }
}