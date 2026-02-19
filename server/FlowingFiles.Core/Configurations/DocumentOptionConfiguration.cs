using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlowingFiles.Core.Models;

namespace FlowingFiles.Core.Configurations
{
    internal class DocumentOptionConfiguration : IEntityTypeConfiguration<DocumentOption>
    {
        public void Configure(EntityTypeBuilder<DocumentOption> builder)
        {
            builder.ToTable("DocumentOption");

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.Path)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(e => e.Required)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.Position)
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}
