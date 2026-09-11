using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlowingFiles.Core.Models;

namespace FlowingFiles.Core.Configurations;

internal class DocumentSampleConfiguration : IEntityTypeConfiguration<DocumentSample>
{
    public void Configure(EntityTypeBuilder<DocumentSample> builder)
    {
        builder.ToTable("DocumentSample");

        builder.Property(e => e.SourceFileName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.ExtractedText)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.HasOne(e => e.DocumentOption)
            .WithMany()
            .HasForeignKey(e => e.DocumentOptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
