using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FargoSecurity.Api.Models.Entities
{
    public class SystemConfiguration: IEntityTypeConfiguration<System>
    {
        public void Configure(EntityTypeBuilder<System> builder)
        {
            builder.HasKey(p => p.SystemId);
            builder.Property(p => p.SystemType).IsRequired().HasMaxLength(3);
            builder.Property(p => p.SystemId).ValueGeneratedOnAdd();
            builder.Property(p => p.Code).IsRequired().HasMaxLength(6);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(300);
            builder.Property(p => p.Path).IsRequired().HasMaxLength(1500);
            builder.Property(p => p.Status).IsRequired().HasDefaultValue(1);
        }
    }
}
