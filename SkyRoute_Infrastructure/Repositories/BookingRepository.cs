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

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        using var transaction = await context.Database.BeginTransactionAsync();

        try

        {

            Flight? flight = await context.Flights.Where(p => p.Id == booking.FlightId).FirstOrDefaultAsync();

            if (flight == null || flight.SeatsFree < booking.PassengerCount)

            {

                return null;

            }

            flight.SeatsFree = flight.SeatsFree - booking.PassengerCount;

            await context.Bookings.AddAsync(booking);

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return booking;

            //I'd like to add a logging here when it's all set up

        } catch (Exception e)
        {
            await transaction.RollbackAsync();

            //I'd like to add a logging method here when I set it all up

        }

        return null;

    }
    /*
    public async Task<List<Booking>> GetAllBookingsAsync()
    {
        return await _context.Bookings.AsNoTracking().ToListAsync();
    }

    

    public async Task<Booking?> GetByReferenceAsync(string referenceCode)
    {
        return await _context.Bookings
            .Include(b => b.Passengers)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.ReferenceCode == referenceCode);
    }
    */
}