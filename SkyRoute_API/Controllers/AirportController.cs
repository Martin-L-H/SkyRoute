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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AirportSearchResponseDTO>>> GetAirports([FromQuery] AirportSearchRequestDTO requestDTO)
    {

        var airports = await _airportService.GetAirportsWithDetailsAsync(requestDTO.CityName, requestDTO.CountryName);

        return Ok(airports);

    }

    [HttpGet]
    [Route("iata")]
    public async Task<ActionResult<AirportSearchResponseDTO>> GetByIATA([FromQuery] AirportSearchRequestDTO requestDTO)
    {

        if (requestDTO.CodeIATA == null)
        {

            return BadRequest("This search requires a valid IATA code");

        }

        var airport = await _airportService.GetAirportByIATAWithDetailsAsync(requestDTO.CodeIATA.ToUpper());
        
        if (airport == null)
        {

            return NotFound(new { message = $"Airport with IATA code '{requestDTO.CodeIATA}' not found." });

        }

        return Ok(airport);

    }
}