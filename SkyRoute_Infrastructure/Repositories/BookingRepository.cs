using Microsoft.EntityFrameworkCore;
using SkyRoute_Domain.Entities;
using SkyRoute_Infrastructure.Context;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Booking>> GetAllBookingsAsync()
    {
        return await _context.Bookings.AsNoTracking().ToListAsync();
    }

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<Booking?> GetByReferenceAsync(string referenceCode)
    {
        return await _context.Bookings
            .Include(b => b.Passengers)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.ReferenceCode == referenceCode);
    }
}