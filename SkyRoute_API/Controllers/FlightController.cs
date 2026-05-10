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
    public async Task<ActionResult<IEnumerable<FlightSearchResponseDTO>>> Search([FromQuery] FlightSearchRequestDTO request)
    {

        if (request.AirportOriginId == request.AirportDestinationId && (request.AirportOriginId != null && request.AirportDestinationId != null))
        {

            return BadRequest("Origin and Destination cannot be the same.");

        }

        if (request.minimumFreeSeats == null)
        {
            request.minimumFreeSeats = 1;
        }

        var validProviders = _flightService.GetAvailableProviders();


        if (request.Provider == "All")
        {

            var allResults = await _flightService.SearchAsync(request);
            return Ok(allResults);

        }

        if (!validProviders.Contains(request.Provider))
        {

            return BadRequest(new{Message = $"'{request.Provider}' is not a valid provider."});

        }

        var results = await _flightService.SearchAsync(request);

        if (results == null || results.Count() == 0)
        {

            return NotFound("No flights found for the selected criteria.");

        }

        return Ok(results);

    }
}