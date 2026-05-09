using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IFlightSearchService _searchService;

    public FlightsController(IFlightSearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FlightSearchResponseDTO>>> Search([FromQuery] FlightSearchRequestDTO requestDTO)
    {

        if (requestDTO.AirportOriginId == requestDTO.AirportDestinationId && (requestDTO.AirportOriginId != null && requestDTO.AirportDestinationId != null))
        {

            return BadRequest("Origin and Destination cannot be the same.");

        }

        var validProviders = _searchService.GetAvailableProviders();

        if (requestDTO.Provider == "All")
        {

            var allResults = await _searchService.SearchAsync(requestDTO);
            return Ok(allResults);

        }

        if (!validProviders.Contains(requestDTO.Provider))
        {

            return BadRequest(new{Message = $"'{requestDTO.Provider}' is not a valid provider."});

        }

        var results = await _searchService.SearchAsync(requestDTO);

        if (results == null || !results.Any())
        {

            return NotFound("No flights found for the selected criteria.");

        }

        return Ok(results);

    }
}