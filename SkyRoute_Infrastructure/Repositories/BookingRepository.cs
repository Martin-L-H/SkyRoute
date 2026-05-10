using Microsoft.EntityFrameworkCore;
using SkyRoute_Domain.Entities;
using SkyRoute_Infrastructure.Context;

public class BookingRepository : IBookingRepository
{

    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public BookingRepository(IDbContextFactory<AppDbContext> contextFactory)
    {

        _contextFactory = contextFactory;

    }

    public async Task<Booking?> CreateBookingAsync(Booking booking)
    {

        using var context = await _contextFactory.CreateDbContextAsync();

        using var transaction = await context.Database.BeginTransactionAsync();

        try

        {
            Flight? flight = await context.Flights.Where(p => p.Id == booking.FlightId).FirstAsync();

            if (flight == null || flight.SeatsFree < booking.PassengerCount)

            {

                return null;

            }
            
            flight.SeatsFree = flight.SeatsFree - booking.PassengerCount;

            context.Bookings.Add(booking);

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return booking;


        } catch (Exception e)
        {
            await transaction.RollbackAsync();

            //I'd like to add a logging method here when I set it all up

        }

        return null;

    }
}