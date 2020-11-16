using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FargoSecurity.Api.Models.Entities
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.HasKey(p => p.RolId);
            builder.Property(p => p.RolId).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(300);
            builder.Property(p => p.Status).IsRequired().HasDefaultValue(1);
            builder.HasOne(p => p.System)
                .WithMany(p => p.Roles);
        }
    }
}
