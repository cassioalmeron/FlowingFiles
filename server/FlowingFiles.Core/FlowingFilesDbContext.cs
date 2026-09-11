using System.Reflection;
using FlowingFiles.Core.Configurations;
using FlowingFiles.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pgvector.EntityFrameworkCore;

namespace FlowingFiles.Core;

public class FlowingFilesDbContext : DbContext
{
    public DbSet<DocumentOption> DocumentOptions { get; set; }
    public DbSet<EmailDestination> EmailDestinations { get; set; }
    public DbSet<DocumentSample> DocumentSamples { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        var dbSettings = new DatabaseSettings();

        if (dbSettings.Provider == DatabaseProvider.PostgreSQL)
            optionsBuilder.UseNpgsql(dbSettings.DatabaseUrl, o => o.UseVector());
        else
            optionsBuilder.UseSqlite($"Data Source={dbSettings.DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        ConfigureEmbedding(modelBuilder.Entity<DocumentSample>().Property(e => e.Embedding));
    }

    // The provider-specific mapping (Postgres `vector(1024)` vs. a plain BLOB) lives in
    // Configurations/*EmbeddingColumnStrategy.cs — this stays a one-line lookup, not a growing
    // if/switch chain, per the Strategy-over-Template-Method guidance the Flowing-EFCore skill gives
    // for provider-specific migration logic (subclassing DbContext per provider would duplicate the
    // context type and complicate the EF Core design-time tooling).
    private void ConfigureEmbedding(PropertyBuilder<float[]> embedding)
    {
        var comparer = new ValueComparer<float[]>(
            (a, b) => a!.SequenceEqual(b!),
            a => a.Aggregate(0, (hash, value) => HashCode.Combine(hash, value)),
            a => a.ToArray());

        IEmbeddingColumnStrategy strategy = Database.IsNpgsql()
            ? new NpgsqlEmbeddingColumnStrategy()
            : new BlobEmbeddingColumnStrategy();

        strategy.Configure(embedding, comparer);
    }
}
