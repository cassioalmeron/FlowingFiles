using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlowingFiles.Core.Models;

namespace FlowingFiles.Core.Configurations
{
    internal class EmailDestinationConfiguration : IEntityTypeConfiguration<EmailDestination>
    {
        public void Configure(EntityTypeBuilder<EmailDestination> builder)
        {
            builder.ToTable("EmailDestination");

            builder.Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(320);

            builder.HasIndex(e => e.EmailAddress)
                .IsUnique();

            builder.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
