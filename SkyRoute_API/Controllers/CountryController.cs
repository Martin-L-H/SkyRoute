using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly ICountryService _countryService;

    public CountryController(ICountryService countryService)
    {

        _countryService = countryService;

    }

    [HttpGet] //Example: https://localhost:7259/api/country or https://localhost:7259/api/country/?Name=sil
    public async Task<IActionResult> GetCountries([FromQuery] CountryDTO request)
    {
        var response = await _countryService.GetCountries(request);

        if (!response.Success)
        {
            return NotFound(response.Message);
        }

        return Ok(response);
    }
}