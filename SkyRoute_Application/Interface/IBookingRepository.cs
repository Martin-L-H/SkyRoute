using SkyRoute_Domain.Entities;

public interface IBookingRepository
{
    //Task<List<Booking>> GetAllBookingsAsync();
    Task<Booking?> CreateBookingAsync(Booking booking);
    //Task<Booking?> GetByReferenceAsync(string referenceCode);
}