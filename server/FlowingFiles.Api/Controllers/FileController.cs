using FlowingFiles.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlowingFiles.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController(FileClassifierService fileClassifierService) : ControllerBase
{
    [HttpPost("classify")]
    public async Task<ActionResult<string[]>> Classify(IFormFileCollection files)
    {
        if (files.Count == 0)
            return BadRequest("At least one file is required.");

        var tempPaths = new List<string>();
        try
        {
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{extension}");
                await using var stream = System.IO.File.Create(tempPath);
                await file.CopyToAsync(stream);
                tempPaths.Add(tempPath);
            }

            var results = await fileClassifierService.ClassifyAllAsync(tempPaths);
            return Ok(results);
        }
        finally
        {
            foreach (var path in tempPaths.Where(System.IO.File.Exists))
                System.IO.File.Delete(path);
        }
    }
}
