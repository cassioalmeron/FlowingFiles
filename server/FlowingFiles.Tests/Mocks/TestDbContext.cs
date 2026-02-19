using FlowingFiles.Core;
using Microsoft.EntityFrameworkCore;

namespace FlowingFiles.Tests.Mocks;

internal class TestDbContext : FlowingFilesDbContext
{
    private static int _count;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder
            .UseInMemoryDatabase($"Virtual-Database-Name-{_count++}")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
}
