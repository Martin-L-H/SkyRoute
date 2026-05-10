public interface IBookingService
{
    Task<ServiceResponse<BookingResponseDTO>> CreateBookingAsync(BookingRequestDTO request);
}