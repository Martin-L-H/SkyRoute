using Microsoft.AspNetCore.Mvc;
using SkyRoute_Application.Interface;


[ApiController]
[Route("api/[controller]")]
public class AirportController : ControllerBase
{

    private readonly IAirportSearchService _airportService;

    public AirportController(IAirportSearchService airportService)
    {

        _airportService = airportService;

    }

    [HttpGet] //Example: https://localhost:7259/api/airport/ or https://localhost:7259/api/airport/?CityName=New york&CountryName=United states&AirportName=Kennedy
    public async Task<IActionResult> GetAirport([FromQuery] AirportSearchRequestDTO request)
    {

        var response = await _airportService.GetAirportsWithDetailsAsync(request);

        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);

    }

    [HttpGet] //Example: https://localhost:7259/api/airports/iata?CodeIata=eze
    [Route("iata")]
    public async Task<IActionResult> GetByIATA([FromQuery] AirportSearchRequestDTO request)
    {

        

        var response = await _airportService.GetAirportByIATAWithDetailsAsync(request);

        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);

    }
}