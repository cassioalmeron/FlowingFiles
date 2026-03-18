using System.Reflection;
using FlowingFiles.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowingFiles.Core;

public class FlowingFilesDbContext : DbContext
{
    public DbSet<DocumentOption> DocumentOptions { get; set; }
    public DbSet<EmailDestination> EmailDestinations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        var dbSettings = new DatabaseSettings();

        if (dbSettings.Provider == DatabaseProvider.PostgreSQL)
            optionsBuilder.UseNpgsql(dbSettings.DatabaseUrl);
        else
            optionsBuilder.UseSqlite($"Data Source={dbSettings.DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
