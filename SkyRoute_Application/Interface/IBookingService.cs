public interface IBookingService
{
    Task<BookingResponseDTO?> CreateBookingAsync(BookingRequestDTO request);
}