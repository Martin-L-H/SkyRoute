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
    public async Task<IActionResult> GetCities([FromQuery] CityDTO request)
    {
        var response = await _cityService.GetCities(request);

        if (!response.Success)
        {
            return NotFound(response.Message);
        }

        return Ok(response);
    }
}