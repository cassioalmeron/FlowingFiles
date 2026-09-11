using FlowingFiles.Core.Models;
using FlowingFiles.Core.Services;
using FlowingFiles.Tests.Mocks;

namespace FlowingFiles.Tests.Services;

[TestClass]
public class DocumentSampleRepositoryTests
{
    private static DocumentSampleRepository CreateRepository(out TestDbContext db)
    {
        db = new TestDbContext();
        return new DocumentSampleRepository(db);
    }

    [TestMethod]
    public async Task NearestAsync_MultipleSamples_ReturnsClosestFirst()
    {
        var repository = CreateRepository(out var db);
        var option = new DocumentOption { Description = "Boleto", Path = "/boleto", Required = false, Position = 1 };
        db.Set<DocumentOption>().Add(option);
        await db.SaveChangesAsync();

        db.Set<DocumentSample>().AddRange(
            new DocumentSample { DocumentOption = option, SourceFileName = "a.pdf", ExtractedText = "a", Embedding = [1, 0, 0], CreatedAt = DateTime.UtcNow },
            new DocumentSample { DocumentOption = option, SourceFileName = "b.pdf", ExtractedText = "b", Embedding = [0, 1, 0], CreatedAt = DateTime.UtcNow },
            new DocumentSample { DocumentOption = option, SourceFileName = "c.pdf", ExtractedText = "c", Embedding = [-1, 0, 0], CreatedAt = DateTime.UtcNow });
        await db.SaveChangesAsync();

        var result = await repository.NearestAsync([1, 0, 0], k: 2);

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("a.pdf", result[0].SourceFileName);
        Assert.AreEqual("b.pdf", result[1].SourceFileName);
    }
}
