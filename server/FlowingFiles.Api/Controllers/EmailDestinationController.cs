using Microsoft.AspNetCore.Mvc;
using FlowingFiles.Core.Dtos;
using FlowingFiles.Core.Services;

namespace FlowingFiles.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailDestinationController : ControllerBase
{
    private readonly ILogger<EmailDestinationController> _logger;
    private readonly EmailDestinationService _service;

    public EmailDestinationController(ILogger<EmailDestinationController> logger, EmailDestinationService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmailDestinationDto>>> GetAll()
    {
        try
        {
            var destinations = await _service.GetAll();
            return Ok(destinations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving email destinations");
            return StatusCode(500, "An error occurred while retrieving email destinations");
        }
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<EmailDestinationDto>>> SaveAll([FromBody] List<EmailDestinationDto> items)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.SaveAll(items);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving email destinations");
            return StatusCode(500, "An error occurred while saving email destinations");
        }
    }
}
