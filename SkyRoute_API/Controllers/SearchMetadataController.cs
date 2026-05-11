using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SearchMetadataController : ControllerBase
{

    private readonly ISearchInitializationService _service;

    public SearchMetadataController(ISearchInitializationService service)
    {

        _service = service;

    }

    [HttpGet]
    public async Task<IActionResult> GetMetadata()
    {

        var response = await _service.GetMetadataAsync();

        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);

    }
}