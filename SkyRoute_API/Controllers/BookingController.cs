using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {

        _bookingService = bookingService;

    }

    [HttpPost] //Example: https://localhost:7259/api/booking
    public async Task<IActionResult> CreateBooking([FromBody] BookingRequestDTO request)
    { 

        var response = await _bookingService.CreateBookingAsync(request);

        if (!response.Success)
        {

            return BadRequest(response.Message);

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