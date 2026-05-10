using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IFlightSearchService _flightService;

    public FlightsController(IFlightSearchService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet] //Example: https://localhost:7259/api/flights/?Provider=All or https://localhost:7259/api/flights/?Provider=BudgetWings&cabintype=Economy&airportoriginid=1
    public async Task<IActionResult> Search([FromQuery] FlightSearchRequestDTO request)
    {


        var response = await _flightService.SearchAsync(request);

        if (!response.Success)
        {
            return NotFound(response.Message);
        }

        return Ok(response);

    }
}