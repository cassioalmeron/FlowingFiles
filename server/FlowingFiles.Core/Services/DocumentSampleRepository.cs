using FlowingFiles.Core.Models;
using Microsoft.EntityFrameworkCore;
using Pgvector;

namespace FlowingFiles.Core.Services;

public class DocumentSampleRepository
{
    private readonly FlowingFilesDbContext _dbContext;

    // FlowingFilesDbContext is scoped (one instance per HTTP request), but FileClassifierService.
    // ClassifyAllAsync runs every file's classification concurrently via Task.WhenAll — without this,
    // two files falling through to the similarity classifier at the same time both call NearestAsync
    // on the same DbContext instance and EF Core throws "A second operation was started on this
    // context instance before a previous operation completed." One instance per scope, so a
    // per-instance lock is enough; it only serializes the DB round trip, not the OCR/PDF-extraction/
    // embedding work happening concurrently around it.
    private readonly SemaphoreSlim _dbLock = new(1, 1);

    public DocumentSampleRepository(FlowingFilesDbContext dbContext) => _dbContext = dbContext;

    public async Task<List<DocumentSample>> NearestAsync(float[] query, int k, CancellationToken cancellationToken = default)
    {
        await _dbLock.WaitAsync(cancellationToken);
        try
        {
            return _dbContext.Database.IsNpgsql()
                ? await NearestNpgsqlAsync(query, k, cancellationToken)
                : await NearestInMemoryAsync(query, k, cancellationToken);
        }
        finally
        {
            _dbLock.Release();
        }
    }

    // Sequential scan by design (Design Decision 3a of plan 003): at the expected sample count an
    // approximate index would be slower to build and less accurate than an exact k-NN scan.
    private async Task<List<DocumentSample>> NearestNpgsqlAsync(float[] query, int k, CancellationToken cancellationToken)
    {
        var vector = new Vector(query);

        return await _dbContext.DocumentSamples
            .FromSqlInterpolated($"""
                SELECT * FROM "DocumentSample"
                ORDER BY "Embedding" <=> {vector}
                LIMIT {k}
                """)
            .Include(s => s.DocumentOption)
            .ToListAsync(cancellationToken);
    }

    // No pgvector equivalent on SQLite (Design Decision 3): load every sample and rank by cosine
    // similarity in memory. Acceptable at the expected corpus size (hundreds of rows).
    private async Task<List<DocumentSample>> NearestInMemoryAsync(float[] query, int k, CancellationToken cancellationToken)
    {
        var samples = await _dbContext.DocumentSamples
            .Include(s => s.DocumentOption)
            .ToListAsync(cancellationToken);

        return samples
            .OrderByDescending(s => VectorMath.CosineSimilarity(query, s.Embedding))
            .Take(k)
            .ToList();
    }
}
