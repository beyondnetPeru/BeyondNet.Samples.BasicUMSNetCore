using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FargoSecurity.Api.Models.Entities
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(p => p.AssignmentId);
            builder.Property(p => p.AssignmentId).ValueGeneratedOnAdd();
            builder.Property(p => p.CanRead).IsRequired().HasDefaultValue(true);
            builder.Property(p => p.Status).IsRequired().HasDefaultValue(1);
        }
    }
}
