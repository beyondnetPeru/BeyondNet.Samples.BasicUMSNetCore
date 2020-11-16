using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FargoSecurity.Api.Models.Entities
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(p => p.ProfileId);
            builder.Property(p => p.ProfileId).ValueGeneratedOnAdd();
        }
    }
}
