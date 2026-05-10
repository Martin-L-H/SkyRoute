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

    [HttpPost] //Example: https://localhost:7259/api/bookings
    public async Task<IActionResult> CreateBooking([FromBody] BookingRequestDTO request)
    { 

        var response = await _bookingService.CreateBookingAsync(request);

        if (!response.Success)
        {

            return NotFound(response.Message);

        }

        return Ok(response);

    }
}

/*

{
  "flightId": 2,
  "providerName": "GlobalAir",
  "passengerCount": 1,
  "passengerList": [
    {
      "firstname": "John",
      "lastname": "doe",
      "email": "johndoe@example.com",
      "documentNumber": "AB1234567",
      "ispassport": true
    }
  ]
}

*/