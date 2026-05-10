using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FlightController : ControllerBase
{
    private readonly IFlightSearchService _flightService;

    public FlightController(IFlightSearchService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet] //Example: https://localhost:7259/api/flight/?Provider=All or https://localhost:7259/api/flight/?Provider=BudgetWings&cabintype=Economy&airportoriginid=1
    public async Task<IActionResult> Search([FromQuery] FlightSearchRequestDTO request)
    {


        var response = await _flightService.SearchAsync(request);

        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);

    }
}