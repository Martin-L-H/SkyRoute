using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkyRoute_Domain.Entities;
using Xunit;

using SkyRoute_Infrastructure.Context;

namespace SkyRoute_Tests.Support;

/// <summary>What the seed step created; handed to every caller and to the final assertions.</summary>
internal sealed record SeededFlight(int FlightId, string ProviderName, int SeatsTotal);

/// <summary>
/// Scenario: ONE domestic flight with a small, fixed number of seats, and every caller tries to
/// book 1-3 passengers on it. The service does "read SeatsFree -> check -> write booking", so if
/// nothing makes that atomic, concurrent callers will all see free seats and overbook the flight.
/// </summary>
internal static class BookingScenario
{
    private const int SeatsOnFlight = 100;

    public static async Task<SeededFlight> SeedAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();

        // Use a provider name the service really knows, so the "provider exists" check passes.
        var provider = scope.ServiceProvider.GetServices<IFlightProvider>().First();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Both airports share one city => same country => domestic => no passport rule involved.
        var city = new City { Name = "Test City", Country = new Country { Name = "Testland" } };

        var flight = new Flight
        {
            FlightNumber = "SR0001",
            TimeDeparture = new DateTime(2030, 1, 1, 10, 0, 0, DateTimeKind.Utc),
            TimeArrival = new DateTime(2030, 1, 1, 12, 0, 0, DateTimeKind.Utc),
            BaseFare = 100m,
            CabinType = default,
            ProviderName = provider.Provider,
            AirportOrigin = new Airport { PublicName = "Test Origin", CodeIATA = "AAA", City = city },
            AirportDestination = new Airport { PublicName = "Test Destination", CodeIATA = "BBB", City = city },
            SeatsTotal = SeatsOnFlight,
            SeatsFree = SeatsOnFlight,
            DurationMinutes = 120,
        };

        db.Add(flight); // adds the whole graph: airports, city, country
        await db.SaveChangesAsync();

        return new SeededFlight(flight.Id, flight.ProviderName, SeatsOnFlight);
    }

    private static BookingRequestDTO BuildRequest(SeededFlight seed, int index)
    {
        var passengers = index % 3 + 1;

        return new BookingRequestDTO
        {
            FlightId = seed.FlightId,
            ProviderName = seed.ProviderName,
            PassengerCount = passengers,
            PassengerList = Enumerable.Range(0, passengers)
                .Select(p => new PassengerRequestDTO
                {
                    FirstName = "Test",
                    LastName = "Passenger",
                    Email = $"p{index}-{p}@test.local",
                    DocumentNumber = $"D{index}-{p}",
                    IsPassport = true,
                })
                .ToList(),
        };
    }

    /// <summary>One caller = one DI scope = one DbContext, exactly like one HTTP request in the API.</summary>
    public static async Task<bool> InvokeAsync(IServiceProvider services, SeededFlight seed, int index)
    {
        await using var scope = services.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<IBookingService>();

        var result = await service.CreateBookingAsync(BuildRequest(seed, index));

        return ResultInspector.IsSuccess(result);
    }

    /// <summary>Checks the database AFTER the storm. Every violated invariant is reported at once.</summary>
    public static async Task AssertInvariantsAsync(IServiceProvider services, SeededFlight seed, RaceReport report)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var bookings = db.Set<Booking>().AsNoTracking().Where(b => b.FlightId == seed.FlightId);
        var bookingCount = await bookings.CountAsync();
        var bookedPassengers = await bookings.SumAsync(b => b.PassengerCount);
        var seatsFree = await db.Set<Flight>().AsNoTracking()
            .Where(f => f.Id == seed.FlightId).Select(f => f.SeatsFree).SingleAsync();

        var problems = new List<string>();

        if (bookingCount != report.Succeeded)
            problems.Add($"Service reported {report.Succeeded:N0} successes but {bookingCount:N0} bookings are stored (lost writes or phantom successes).");

        if (bookedPassengers > seed.SeatsTotal)
            problems.Add($"OVERBOOKED: {bookedPassengers:N0} passengers booked on a flight with {seed.SeatsTotal} seats.");

        if (seatsFree < 0)
            problems.Add($"Flight.SeatsFree went negative ({seatsFree}).");

        if (seatsFree != seed.SeatsTotal - bookedPassengers)
            problems.Add($"Flight.SeatsFree is {seatsFree} but {seed.SeatsTotal} - {bookedPassengers} booked = {seed.SeatsTotal - bookedPassengers} (counter out of sync with bookings).");

        Assert.True(problems.Count == 0, Environment.NewLine + string.Join(Environment.NewLine, problems));
    }
}