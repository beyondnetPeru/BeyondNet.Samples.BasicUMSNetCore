using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FargoSecurity.Api.Models.Entities
{
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.HasKey(p => p.OptionId);
            builder.Property(p => p.OptionId).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(300);
            builder.Property(p => p.Path).IsRequired().HasMaxLength(1500);
            builder.Property(p => p.Status).IsRequired().HasDefaultValue(1);
        }
    }
}
