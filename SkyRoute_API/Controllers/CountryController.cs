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
    public async Task<ActionResult<IEnumerable<CountryDTO>>> GetCountries([FromQuery] CountryDTO request)
    {
        var countries = await _countryService.GetCountries(request);

        if (countries == null || countries.Count() == 0)
        {
            return NotFound("No countries found!");
        }

        return Ok(countries);
    }
}