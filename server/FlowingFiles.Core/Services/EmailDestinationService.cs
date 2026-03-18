using FlowingFiles.Core.Dtos;
using FlowingFiles.Core.Extensions;
using FlowingFiles.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowingFiles.Core.Services;

public class EmailDestinationService
{
    private readonly FlowingFilesDbContext _dbContext;

    public EmailDestinationService(FlowingFilesDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<EmailDestinationDto>> GetAll()
    {
        var entities = await _dbContext.Set<EmailDestination>()
            .OrderBy(e => e.EmailAddress)
            .ToListAsync();

        return entities.Select(e => e.CopyTo<EmailDestinationDto>());
    }

    public async Task<IEnumerable<EmailDestinationDto>> SaveAll(List<EmailDestinationDto> items)
    {
        var existing = await _dbContext.Set<EmailDestination>()
            .AsTracking()
            .ToListAsync();

        var existingById = existing.ToDictionary(e => e.Id);
        var submittedIds = items.Where(i => i.Id > 0).Select(i => i.Id).ToHashSet();

        // Delete: items in DB absent from request
        var toDelete = existing.Where(e => !submittedIds.Contains(e.Id)).ToList();
        _dbContext.Set<EmailDestination>().RemoveRange(toDelete);

        // Update: items with id > 0 that exist in DB
        foreach (var dto in items.Where(i => i.Id > 0))
        {
            if (!existingById.TryGetValue(dto.Id, out var entity)) continue;
            entity.EmailAddress = dto.EmailAddress;
            entity.Active = dto.Active;
        }

        // Create: items with id <= 0 (new items from frontend)
        var created = new List<EmailDestination>();
        foreach (var dto in items.Where(i => i.Id <= 0))
        {
            var entity = new EmailDestination
            {
                EmailAddress = dto.EmailAddress,
                Active = dto.Active
            };
            created.Add(entity);
            _dbContext.Set<EmailDestination>().Add(entity);
        }

        await _dbContext.SaveChangesAsync();

        return existing
            .Where(e => submittedIds.Contains(e.Id))
            .Concat(created)
            .OrderBy(e => e.EmailAddress)
            .Select(e => e.CopyTo<EmailDestinationDto>());
    }
}
