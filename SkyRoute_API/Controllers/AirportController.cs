using Microsoft.AspNetCore.Mvc;
using SkyRoute_Application.Interface;


[ApiController]
[Route("api/[controller]")]
public class AirportsController : ControllerBase
{

    private readonly IAirportSearchService _airportService;

    public AirportsController(IAirportSearchService airportService)
    {

        _airportService = airportService;

    }

    [HttpGet] //Example: https://localhost:7259/api/airports/ or https://localhost:7259/api/airports/?CityName=New york&CountryName=United states&AirportName=Kennedy
    public async Task<ActionResult<IEnumerable<AirportSearchResponseDTO>>> GetAirports([FromQuery] AirportSearchRequestDTO request)
    {

        var airports = await _airportService.GetAirportsWithDetailsAsync(request);

        if (airports == null || airports.Count() == 0)
        {
            return NotFound("No airports found!");
        }

        return Ok(airports);

    }

    [HttpGet] //Example: https://localhost:7259/api/airports/iata?CodeIata=eze
    [Route("iata")]
    public async Task<ActionResult<AirportSearchResponseDTO>> GetByIATA([FromQuery] AirportSearchRequestDTO request)
    {

        if (request.CodeIATA == null || request.CodeIATA == "")
        {

            return BadRequest("This search requires a valid IATA code");

        }

        var airport = await _airportService.GetAirportByIATAWithDetailsAsync(request.CodeIATA.ToUpper());
        
        if (airport == null)
        {

            return NotFound(new { message = $"Airport with IATA code '{request.CodeIATA}' not found." });

        }

        return Ok(airport);

    }
}