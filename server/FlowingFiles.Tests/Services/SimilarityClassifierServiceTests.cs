using System.Net;
using System.Net.Http.Json;
using FlowingFiles.Core;
using FlowingFiles.Core.Models;
using FlowingFiles.Core.Services;
using FlowingFiles.Tests.Mocks;
using Microsoft.Extensions.Options;

namespace FlowingFiles.Tests.Services;

[TestClass]
public class SimilarityClassifierServiceTests
{
    private static SimilarityClassifierService CreateService(out TestDbContext db, float[] queryEmbedding, double threshold, int k)
    {
        db = new TestDbContext();
        var repository = new DocumentSampleRepository(db);
        var settings = Options.Create(new OllamaSettings
        {
            BaseUrl = "http://ollama.local",
            EmbeddingModel = "bge-m3",
            Threshold = threshold,
            K = k
        });

        var httpClient = new HttpClient(new FakeEmbeddingHandler(queryEmbedding)) { BaseAddress = new Uri(settings.Value.BaseUrl) };
        var embeddingService = new EmbeddingService(httpClient, settings);

        return new SimilarityClassifierService(repository, embeddingService, settings);
    }

    [TestMethod]
    public async Task ClassifyAsync_NoSamples_ReturnsUnknown()
    {
        var service = CreateService(out _, [1, 0, 0], threshold: 0.5, k: 5);

        var result = await service.ClassifyAsync("some text");

        Assert.AreEqual("Unknown", result.Label);
        Assert.AreEqual(0, result.Confidence);
    }

    [TestMethod]
    public async Task ClassifyAsync_ClearMajority_ReturnsLabelAboveThreshold()
    {
        var service = CreateService(out var db, [1, 0, 0], threshold: 0.5, k: 3);
        var boleto = new DocumentOption { Description = "Boleto", Path = "/boleto", Required = false, Position = 1 };
        var nfse = new DocumentOption { Description = "NFSE", Path = "/nfse", Required = false, Position = 2 };
        db.Set<DocumentOption>().AddRange(boleto, nfse);
        await db.SaveChangesAsync();

        db.Set<DocumentSample>().AddRange(
            new DocumentSample { DocumentOption = boleto, SourceFileName = "a.pdf", ExtractedText = "a", Embedding = [1, 0, 0], CreatedAt = DateTime.UtcNow },
            new DocumentSample { DocumentOption = boleto, SourceFileName = "b.pdf", ExtractedText = "b", Embedding = [0.9f, 0.1f, 0], CreatedAt = DateTime.UtcNow },
            new DocumentSample { DocumentOption = nfse, SourceFileName = "c.pdf", ExtractedText = "c", Embedding = [0, 1, 0], CreatedAt = DateTime.UtcNow });
        await db.SaveChangesAsync();

        var result = await service.ClassifyAsync("some text");

        Assert.AreEqual("Boleto", result.Label);
        Assert.IsTrue(result.Confidence >= 0.5);

        // The full ranking is exposed regardless of which label won, so the UI can show why.
        Assert.AreEqual(3, result.Neighbours.Count);
        Assert.AreEqual("Boleto", result.Neighbours[0].Label);
        Assert.IsTrue(result.Neighbours[0].Similarity >= result.Neighbours[1].Similarity);
        Assert.IsTrue(result.Neighbours[1].Similarity >= result.Neighbours[2].Similarity);
    }

    [TestMethod]
    public async Task ClassifyAsync_SplitVoteBelowThreshold_ReturnsUnknown()
    {
        var service = CreateService(out var db, [1, 0, 0], threshold: 0.9, k: 2);
        var boleto = new DocumentOption { Description = "Boleto", Path = "/boleto", Required = false, Position = 1 };
        var nfse = new DocumentOption { Description = "NFSE", Path = "/nfse", Required = false, Position = 2 };
        db.Set<DocumentOption>().AddRange(boleto, nfse);
        await db.SaveChangesAsync();

        // Both samples are equally similar to the query but disagree on the label, so the winning
        // share of the vote (0.5) falls below a 0.9 threshold regardless of tie-breaking.
        db.Set<DocumentSample>().AddRange(
            new DocumentSample { DocumentOption = boleto, SourceFileName = "a.pdf", ExtractedText = "a", Embedding = [1, 0, 0], CreatedAt = DateTime.UtcNow },
            new DocumentSample { DocumentOption = nfse, SourceFileName = "b.pdf", ExtractedText = "b", Embedding = [1, 0, 0], CreatedAt = DateTime.UtcNow });
        await db.SaveChangesAsync();

        var result = await service.ClassifyAsync("some text");

        Assert.AreEqual("Unknown", result.Label);
    }
}

file class FakeEmbeddingHandler(float[] embedding) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new { embedding })
        };
        return Task.FromResult(response);
    }
}
