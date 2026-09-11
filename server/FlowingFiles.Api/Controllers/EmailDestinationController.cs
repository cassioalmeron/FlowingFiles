using Microsoft.AspNetCore.Mvc;
using FlowingFiles.Core.Dtos;
using FlowingFiles.Core.Services;

namespace FlowingFiles.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailDestinationController : ControllerBase
{
    private readonly EmailDestinationService _service;

    public EmailDestinationController(EmailDestinationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmailDestinationDto>>> GetAll()
    {
        var destinations = await _service.GetAll();
        return Ok(destinations);
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<EmailDestinationDto>>> SaveAll([FromBody] List<EmailDestinationDto> items)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.SaveAll(items);
        return Ok(result);
    }
}
