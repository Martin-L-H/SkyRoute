using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {

        _cityService = cityService;

    }

    [HttpGet] //Example: https://localhost:7259/api/city or https://localhost:7259/api/city?Name=A&CountryName=A
    public async Task<ActionResult<IEnumerable<CityDTO>>> GetCities([FromQuery] CityDTO request)
    {
        var cities = await _cityService.GetCities(request);

        if (cities == null || cities.Count() == 0)
        {
            return NotFound("No cities found!");
        }

        return Ok(cities);
    }
}