using Microsoft.AspNetCore.Mvc;
using FlowingFiles.Core.Dtos;
using FlowingFiles.Core.Services;

namespace FlowingFiles.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentOptionController : ControllerBase
{
    private readonly ILogger<DocumentOptionController> _logger;
    private readonly DocumentOptionService _service;

    public DocumentOptionController(ILogger<DocumentOptionController> logger, DocumentOptionService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentOptionDto>>> GetAll()
    {
        try
        {
            var options = await _service.GetAll();
            return Ok(options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving document options");
            return StatusCode(500, "An error occurred while retrieving document options");
        }
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<DocumentOptionDto>>> SaveAll([FromBody] List<DocumentOptionDto> items)
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
            _logger.LogError(ex, "Error saving document options");
            return StatusCode(500, "An error occurred while saving document options");
        }
    }
}
