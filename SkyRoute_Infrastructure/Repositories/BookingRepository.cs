using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkyRoute_Domain.Entities;
using SkyRoute_Infrastructure.Context;

public class BookingRepository : IBookingRepository
{

    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly ILogger<BookingRepository> _logger;

    public BookingRepository(IDbContextFactory<AppDbContext> contextFactory, ILogger<BookingRepository> logger)
    {

        _contextFactory = contextFactory;
        _logger = logger;

    }

    public async Task<Booking?> CreateBookingAsync(Booking booking)
    {

        using var context = await _contextFactory.CreateDbContextAsync();

        using var transaction = await context.Database.BeginTransactionAsync();

        try

        {
            Flight? flight = await context.Flights.Where(p => p.Id == booking.FlightId).SingleOrDefaultAsync();

            if (flight == null || flight.SeatsFree < booking.PassengerCount)

            {

                _logger.LogError("Repository: Flight not found or insufficient seats for booking {FlightId}", booking.FlightId);

                return null;

            }
            //Using a job could make this insertion atomic to avoid accidental overbooking
            flight.SeatsFree = flight.SeatsFree - booking.PassengerCount;

            context.Bookings.Add(booking);

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return booking;


        } catch (Exception e)
        {
            await transaction.RollbackAsync();

            _logger.LogError(e, "Repository: Exception during booking insertion for flight {FlightId}", booking.FlightId);

        }

        return null;

    }
}