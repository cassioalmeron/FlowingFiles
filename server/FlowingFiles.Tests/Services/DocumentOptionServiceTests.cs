using FlowingFiles.Core.Dtos;
using FlowingFiles.Core.Models;
using FlowingFiles.Core.Services;
using FlowingFiles.Tests.Mocks;

namespace FlowingFiles.Tests.Services;

[TestClass]
public class DocumentOptionServiceTests
{
    private static DocumentOptionService CreateService(out TestDbContext db)
    {
        db = new TestDbContext();
        return new DocumentOptionService(db);
    }

    private static async Task SeedAsync(TestDbContext db, params DocumentOption[] options)
    {
        db.Set<DocumentOption>().AddRange(options);
        await db.SaveChangesAsync();
    }

    [TestMethod]
    public async Task GetAll_EmptyDatabase_ReturnsEmptyList()
    {
        var service = CreateService(out _);

        var result = await service.GetAll();

        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task GetAll_ReturnsItemsOrderedByPosition()
    {
        var service = CreateService(out var db);
        await SeedAsync(db,
            new DocumentOption { Description = "B", Path = "/b", Required = false, Position = 2 },
            new DocumentOption { Description = "A", Path = "/a", Required = true, Position = 1 });

        var result = await service.GetAll();

        var list = result.ToList();
        Assert.AreEqual(2, list.Count);
        Assert.AreEqual("A", list[0].Description);
        Assert.AreEqual("B", list[1].Description);
    }

    [TestMethod]
    public async Task SaveAll_NewItems_AreCreatedWithServerIds()
    {
        var service = CreateService(out _);
        var items = new List<DocumentOptionDto>
        {
            new() { Id = -1, Description = "Doc A", Path = "/a", Required = true, Position = 1 },
            new() { Id = -2, Description = "Doc B", Path = "/b", Required = false, Position = 2 },
        };

        var result = await service.SaveAll(items);

        var list = result.ToList();
        Assert.AreEqual(2, list.Count);
        Assert.IsTrue(list.All(r => r.Id > 0), "All returned items should have server-assigned IDs");
        Assert.AreEqual("Doc A", list[0].Description);
        Assert.AreEqual("Doc B", list[1].Description);
    }

    [TestMethod]
    public async Task SaveAll_ExistingItems_AreUpdated()
    {
        var service = CreateService(out var db);
        await SeedAsync(db,
            new DocumentOption { Description = "Original", Path = "/old", Required = false, Position = 1 });

        var existing = (await service.GetAll()).First();
        var items = new List<DocumentOptionDto>
        {
            new() { Id = existing.Id, Description = "Updated", Path = "/new", Required = true, Position = 1 },
        };

        var result = await service.SaveAll(items);

        var updated = result.First();
        Assert.AreEqual(existing.Id, updated.Id);
        Assert.AreEqual("Updated", updated.Description);
        Assert.AreEqual("/new", updated.Path);
        Assert.IsTrue(updated.Required);
    }

    [TestMethod]
    public async Task SaveAll_ItemsAbsentFromRequest_AreDeleted()
    {
        var service = CreateService(out var db);
        await SeedAsync(db,
            new DocumentOption { Description = "Keep", Path = "/keep", Required = false, Position = 1 },
            new DocumentOption { Description = "Remove", Path = "/remove", Required = false, Position = 2 });

        var all = (await service.GetAll()).ToList();
        var keep = all.First(x => x.Description == "Keep");

        var items = new List<DocumentOptionDto>
        {
            new() { Id = keep.Id, Description = keep.Description, Path = keep.Path, Required = keep.Required, Position = 1 },
        };

        var result = await service.SaveAll(items);

        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("Keep", result.First().Description);
    }

    [TestMethod]
    public async Task SaveAll_EmptyList_DeletesAllItems()
    {
        var service = CreateService(out var db);
        await SeedAsync(db,
            new DocumentOption { Description = "A", Path = "/a", Required = false, Position = 1 });

        var result = await service.SaveAll([]);

        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public async Task SaveAll_ResultIsOrderedByPosition()
    {
        var service = CreateService(out var db);
        await SeedAsync(db,
            new DocumentOption { Description = "First", Path = "/first", Required = false, Position = 1 });

        var existing = (await service.GetAll()).First();
        var items = new List<DocumentOptionDto>
        {
            new() { Id = -1, Description = "Third", Path = "/third", Required = false, Position = 3 },
            new() { Id = existing.Id, Description = "First", Path = "/first", Required = false, Position = 1 },
            new() { Id = -2, Description = "Second", Path = "/second", Required = false, Position = 2 },
        };

        var result = await service.SaveAll(items);

        var list = result.ToList();
        Assert.AreEqual(3, list.Count);
        Assert.AreEqual("First", list[0].Description);
        Assert.AreEqual("Second", list[1].Description);
        Assert.AreEqual("Third", list[2].Description);
    }
}
