using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FargoSecurity.Api.Models.Entities
{
    public class AssignmentItemConfiguration : IEntityTypeConfiguration<AssignmentItem>
    {
        public void Configure(EntityTypeBuilder<AssignmentItem> builder)
        {
            builder.HasKey(p => p.AssignmentItemId);
            builder.Property(p => p.AssignmentItemId).ValueGeneratedOnAdd();
            builder.Property(p => p.CanRead).IsRequired().HasDefaultValue(true);
            builder.Property(p => p.Status).IsRequired().HasDefaultValue(1);
        }
    }
}
