using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FargoSecurity.Api.Models.Entities
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.UserId);
            builder.Property(p => p.UserId).ValueGeneratedOnAdd();
            builder.Property(p => p.UserType).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(1000);
            builder.Property(p => p.UserName).IsRequired().HasMaxLength(300);
            builder.Property(p => p.Password).IsRequired().HasMaxLength(3000);
            builder.Property(p => p.Status).IsRequired().HasDefaultValue(1);
        }
    }
}
