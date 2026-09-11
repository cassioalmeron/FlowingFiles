using FlowingFiles.Core.Dtos;
using Microsoft.Extensions.Options;

namespace FlowingFiles.Core.Services;

public class SimilarityClassifierService(
    DocumentSampleRepository repository,
    EmbeddingService embeddingService,
    IOptions<OllamaSettings> settings)
{
    public async Task<ClassificationResult> ClassifyAsync(string text, CancellationToken cancellationToken = default)
    {
        var vector = await embeddingService.EmbedAsync(text, cancellationToken);
        var neighbours = await repository.NearestAsync(vector, settings.Value.K, cancellationToken);

        if (neighbours.Count == 0)
            return new ClassificationResult("Unknown", 0, []);

        var scored = neighbours
            .Select(n => new NeighbourScore(n.DocumentOption.Description, VectorMath.CosineSimilarity(vector, n.Embedding)))
            .OrderByDescending(n => n.Similarity)
            .ToList();

        var totalWeight = scored.Sum(n => n.Similarity);

        var best = scored
            .GroupBy(n => n.Label)
            .Select(g => (Label: g.Key, Weight: g.Sum(n => n.Similarity)))
            .OrderByDescending(g => g.Weight)
            .First();

        var confidence = totalWeight > 0 ? best.Weight / totalWeight : 0;

        return confidence >= settings.Value.Threshold
            ? new ClassificationResult(best.Label, confidence, scored)
            : new ClassificationResult("Unknown", confidence, scored);
    }
}
