using FlowingFiles.Core.Dtos;
using FlowingFiles.Core.Extensions;
using FlowingFiles.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowingFiles.Core.Services;

public class DocumentOptionService
{
    private readonly FlowingFilesDbContext _dbContext;

    public DocumentOptionService(FlowingFilesDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<DocumentOptionDto>> GetAll()
    {
        var entities = await _dbContext.Set<DocumentOption>()
            .OrderBy(o => o.Position)
            .ToListAsync();

        return entities.Select(e => e.CopyTo<DocumentOptionDto>());
    }

    public async Task<IEnumerable<DocumentOptionDto>> SaveAll(List<DocumentOptionDto> items)
    {
        var existing = await _dbContext.Set<DocumentOption>()
            .AsTracking()
            .ToListAsync();

        var existingById = existing.ToDictionary(e => e.Id);
        var submittedIds = items.Where(i => i.Id > 0).Select(i => i.Id).ToHashSet();

        // Delete: items in DB absent from request
        var toDelete = existing.Where(e => !submittedIds.Contains(e.Id)).ToList();
        _dbContext.Set<DocumentOption>().RemoveRange(toDelete);

        // Update: items with id > 0 that exist in DB
        foreach (var dto in items.Where(i => i.Id > 0))
        {
            if (!existingById.TryGetValue(dto.Id, out var entity)) continue;
            entity.Description = dto.Description;
            entity.Path = dto.Path;
            entity.Required = dto.Required;
            entity.Position = dto.Position;
        }

        // Create: items with id <= 0 (new items from frontend)
        var created = new List<DocumentOption>();
        foreach (var dto in items.Where(i => i.Id <= 0))
        {
            var entity = new DocumentOption
            {
                Description = dto.Description,
                Path = dto.Path,
                Required = dto.Required,
                Position = dto.Position
            };
            created.Add(entity);
            _dbContext.Set<DocumentOption>().Add(entity);
        }

        await _dbContext.SaveChangesAsync();

        return existing
            .Where(e => submittedIds.Contains(e.Id))
            .Concat(created)
            .OrderBy(e => e.Position)
            .Select(e => e.CopyTo<DocumentOptionDto>());
    }
}
