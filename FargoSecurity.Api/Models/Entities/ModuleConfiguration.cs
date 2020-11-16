using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FargoSecurity.Api.Models.Entities
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasKey(p => p.ModuleId);
            builder.Property(p => p.ModuleId).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(300);
            builder.Property(p => p.Path).IsRequired().HasMaxLength(1500);
            builder.Property(p => p.Status).IsRequired().HasDefaultValue(1);
        }
    }
}
