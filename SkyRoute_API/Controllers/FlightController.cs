using Microsoft.AspNetCore.Mvc;
using SkyRoute_Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IFlightSearchService _searchService;

    public FlightsController(IFlightSearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<FlightSearchResponseDTO>>> Search([FromQuery] FlightSearchRequestDTO requestDTO)
    {

        if (requestDTO.AirportOriginId == requestDTO.AirportDestinationId)
        {
            return BadRequest("Origin and Destination cannot be the same.");
        }

        IEnumerable<FlightSearchResponseDTO> results = await _searchService.SearchAsync(requestDTO);

        if (results == null || !results.Any())
        {
            return NotFound("No flights found for the selected criteria.");
        }

        return Ok(results);

    }
}