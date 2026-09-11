using System.Net.Http.Json;
using FlowingFiles.Core.Dtos;
using Microsoft.Extensions.Options;

namespace FlowingFiles.Core.Services;

public class EmbeddingService(HttpClient httpClient, IOptions<OllamaSettings> settings)
{
    public async Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("/api/embeddings", new
        {
            model = settings.Value.EmbeddingModel,
            prompt = text
        }, cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<EmbeddingResponse>(cancellationToken: cancellationToken);
        return result?.Embedding ?? [];
    }
}
