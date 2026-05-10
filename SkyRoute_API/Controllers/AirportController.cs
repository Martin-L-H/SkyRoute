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
    public async Task<IActionResult> GetAirports([FromQuery] AirportSearchRequestDTO request)
    {

        var response = await _airportService.GetAirportsWithDetailsAsync(request);

        if (!response.Success)
        {
            return NotFound(response.Message);
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
            return NotFound(response.Message);
        }

        return Ok(response);

    }
}