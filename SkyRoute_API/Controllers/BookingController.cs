using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<ActionResult<BookingResponseDTO>> CreateBooking([FromBody] BookingRequestDTO request)
    {

        if (request == null || request.PassengerList == null || request.PassengerList.Count == 0)
        {

            return BadRequest("Invalid Request body, is null.");

        }

        BookingResponseDTO response = await _bookingService.CreateBookingAsync(request);

        if (response == null)
        {

            return BadRequest("Unable to process booking. Please check availability and try again.");

        }
        return Ok(response);
    }
}