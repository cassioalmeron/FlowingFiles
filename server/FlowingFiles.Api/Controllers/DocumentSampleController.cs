using System.IO.Compression;
using System.Text.Json;
using FlowingFiles.Core;
using FlowingFiles.Core.Dtos;
using FlowingFiles.Core.Extensions;
using FlowingFiles.Core.Models;
using FlowingFiles.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowingFiles.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentSampleController : ControllerBase
{
    private readonly ILogger<DocumentSampleController> _logger;
    private readonly FlowingFilesDbContext _dbContext;
    private readonly EmbeddingService _embeddingService;
    private readonly OcrService _ocrService;

    public DocumentSampleController(
        ILogger<DocumentSampleController> logger,
        FlowingFilesDbContext dbContext,
        EmbeddingService embeddingService,
        OcrService ocrService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _embeddingService = embeddingService;
        _ocrService = ocrService;
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<DocumentSampleDto>>> Upload(IFormFileCollection files, [FromForm] int documentOptionId)
    {
        if (files.Count == 0)
            return BadRequest("At least one file is required.");

        var documentOption = await _dbContext.Set<DocumentOption>().FindAsync(documentOptionId);
        if (documentOption == null)
            return BadRequest($"Document option {documentOptionId} not found.");

        var tempPaths = new List<string>();
        try
        {
            var created = new List<DocumentSample>();

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{extension}");
                await using (var stream = System.IO.File.Create(tempPath))
                    await file.CopyToAsync(stream);
                tempPaths.Add(tempPath);

                created.Add(await BuildSampleAsync(documentOptionId, file.FileName, tempPath));
            }

            _dbContext.Set<DocumentSample>().AddRange(created);
            await _dbContext.SaveChangesAsync();

            return Ok(created.Select(s => s.CopyTo<DocumentSampleDto>()));
        }
        finally
        {
            foreach (var path in tempPaths.Where(System.IO.File.Exists))
                System.IO.File.Delete(path);
        }
    }

    // Batch backfill (plan 003, Phase 6): the client already parsed the archive and decided which
    // entry goes to which DocumentOption using the same matching logic as the interactive ZIP import
    // (web/src/lib/zip.ts) — this endpoint only extracts exactly the entries it is told about and
    // ingests them. It does not re-derive the path-to-slot mapping server-side, so there is exactly
    // one implementation of that matching logic in the whole app.
    [HttpPost("import-zip")]
    public async Task<ActionResult<ImportZipResultDto>> ImportZip(IFormFile archive, [FromForm] string assignments)
    {
        if (archive == null || archive.Length == 0)
            return BadRequest("An archive file is required.");

        List<ZipAssignment>? parsedAssignments;
        try
        {
            parsedAssignments = JsonSerializer.Deserialize<List<ZipAssignment>>(
                assignments, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException)
        {
            return BadRequest("Invalid assignments payload.");
        }

        if (parsedAssignments == null || parsedAssignments.Count == 0)
            return BadRequest("At least one assignment is required.");

        var optionIds = parsedAssignments.Select(a => a.DocumentOptionId).Distinct().ToList();
        var validOptionIds = (await _dbContext.Set<DocumentOption>()
            .Where(o => optionIds.Contains(o.Id))
            .Select(o => o.Id)
            .ToListAsync())
            .ToHashSet();

        var tempFiles = new List<string>();
        var failed = new List<ZipImportFailureDto>();
        var created = new List<DocumentSample>();

        try
        {
            await using var archiveStream = archive.OpenReadStream();
            using var zip = new ZipArchive(archiveStream, ZipArchiveMode.Read);

            foreach (var assignment in parsedAssignments)
            {
                if (!validOptionIds.Contains(assignment.DocumentOptionId))
                {
                    failed.Add(new ZipImportFailureDto(assignment.Path, "Unknown document option"));
                    continue;
                }

                var normalizedPath = assignment.Path.Replace('\\', '/').TrimStart('/');
                var entry = zip.Entries.FirstOrDefault(e =>
                    e.FullName.Replace('\\', '/').Equals(normalizedPath, StringComparison.OrdinalIgnoreCase));

                if (entry == null)
                {
                    failed.Add(new ZipImportFailureDto(assignment.Path, "Entry not found in archive"));
                    continue;
                }

                var extension = Path.GetExtension(entry.Name).ToLower();
                var tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{extension}");
                tempFiles.Add(tempFilePath);

                try
                {
                    await using (var entryStream = entry.Open())
                    await using (var fileStream = System.IO.File.Create(tempFilePath))
                        await entryStream.CopyToAsync(fileStream);

                    created.Add(await BuildSampleAsync(assignment.DocumentOptionId, entry.Name, tempFilePath));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to ingest {Path} from batch archive", assignment.Path);
                    failed.Add(new ZipImportFailureDto(assignment.Path, "Extraction or embedding failed"));
                }
            }

            _dbContext.Set<DocumentSample>().AddRange(created);
            await _dbContext.SaveChangesAsync();

            return Ok(new ImportZipResultDto(parsedAssignments.Count, created.Count, failed));
        }
        finally
        {
            foreach (var path in tempFiles.Where(System.IO.File.Exists))
                System.IO.File.Delete(path);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentSampleGroupDto>>> GetAll()
    {
        var samples = await _dbContext.Set<DocumentSample>()
            .Include(s => s.DocumentOption)
            .ToListAsync();

        var grouped = samples
            .GroupBy(s => s.DocumentOption)
            .Select(g => new DocumentSampleGroupDto(
                g.Key.Id,
                g.Key.Description,
                g.Count(),
                g.Select(s => s.CopyTo<DocumentSampleDto>()).ToList()))
            .OrderBy(g => g.DocumentOptionDescription);

        return Ok(grouped);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DocumentSampleDetailDto>> GetById(int id)
    {
        var sample = await _dbContext.Set<DocumentSample>().FindAsync(id);
        if (sample == null)
            return NotFound();

        return Ok(sample.CopyTo<DocumentSampleDetailDto>());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var sample = await _dbContext.Set<DocumentSample>().FindAsync(id);
        if (sample == null)
            return NotFound();

        _dbContext.Set<DocumentSample>().Remove(sample);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private async Task<DocumentSample> BuildSampleAsync(int documentOptionId, string fileName, string tempFilePath)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        var text = await ExtractTextAsync(tempFilePath, extension);
        var embedding = await _embeddingService.EmbedAsync(text);

        return new DocumentSample
        {
            DocumentOptionId = documentOptionId,
            SourceFileName = fileName,
            ExtractedText = text,
            Embedding = embedding,
            CreatedAt = DateTime.UtcNow
        };
    }

    private async Task<string> ExtractTextAsync(string filePath, string extension) =>
        extension switch
        {
            ".jpg" or ".jpeg" or ".png" => await _ocrService.ExtractTextAsync(filePath),
            _ => await PdfTextService.ExtractTextAsync(filePath)
        };
}
