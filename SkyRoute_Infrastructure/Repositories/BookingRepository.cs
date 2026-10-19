using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkyRoute_Domain.Entities;
using SkyRoute_Infrastructure.Context;

public class BookingRepository : IBookingRepository
{

    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly ILogger<BookingRepository> _logger;

    private static readonly EventId NoSeatsEvent = new(1001, "BookingNoSeats");
    private static readonly EventId DeadlockEvent = new(1002, "BookingDeadlock");
    private static readonly EventId OtherExceptionEvent = new(1003, "BookingException");
    private const int SqlServerDeadlockVictim = 1205;

    public BookingRepository(IDbContextFactory<AppDbContext> contextFactory, ILogger<BookingRepository> logger)
    {

        _contextFactory = contextFactory;
        _logger = logger;

    }

    public async Task<Booking?> CreateBookingAsync(Booking booking)
    {

        using var context = await _contextFactory.CreateDbContextAsync();

        using var transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

        try

        {
            //OLD METHOD
            /*Flight? flight = await context.Flights.Where(p => p.Id == booking.FlightId).SingleOrDefaultAsync();
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
            */
            //NEW METHOD
            int isBookingConfirmed = await context.Flights
                .Where(p => p.Id == booking.FlightId && p.SeatsFree >= booking.PassengerCount)
                .ExecuteUpdateAsync(f => f.SetProperty(p => p.SeatsFree, p => p.SeatsFree - booking.PassengerCount));
            if (isBookingConfirmed == 0)

            {

                _logger.LogError(NoSeatsEvent, "Repository: Flight not found or insufficient seats for booking {FlightId}", booking.FlightId);

                return null;

            }

            context.Bookings.Add(booking);

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return booking;


        } catch (Exception e)
        {
            try
            {
                await transaction.RollbackAsync();
            } 
            catch (Exception rollbackError)
            {
                _logger.LogWarning(rollbackError, "Repository: rollback failed after an earlier error for flight {FlightId}", booking.FlightId);
            }

            if (IsDeadlockVictim(e))
            {
                _logger.LogError(DeadlockEvent, e, "Repository: DEADLOCK VICTIM (SQL error 1205) for flight {FlightId}", booking.FlightId);
            }
            else
            {
                _logger.LogError(OtherExceptionEvent, e, "Repository: Exception during booking insertion for flight {FlightId}", booking.FlightId);
            }

        }

        return null;

    }

    private static bool IsDeadlockVictim(Exception? exception)
    {
        for (var e = exception; e != null; e = e.InnerException)
        {
            if (e is SqlException sql && sql.Errors.Cast<SqlError>().Any(error => error.Number == SqlServerDeadlockVictim))
            {
                return true;
            }
        }

        return false;
    }
}