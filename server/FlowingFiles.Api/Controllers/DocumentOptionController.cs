using Microsoft.AspNetCore.Mvc;
using FlowingFiles.Core.Dtos;
using FlowingFiles.Core.Services;

namespace FlowingFiles.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentOptionController : ControllerBase
{
    private readonly DocumentOptionService _service;

    public DocumentOptionController(DocumentOptionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentOptionDto>>> GetAll()
    {
        var options = await _service.GetAll();
        return Ok(options);
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<DocumentOptionDto>>> SaveAll([FromBody] List<DocumentOptionDto> items)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.SaveAll(items);
        return Ok(result);
    }
}
