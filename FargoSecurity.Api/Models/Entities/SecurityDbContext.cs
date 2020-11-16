using Microsoft.EntityFrameworkCore;
using FargoSecurity.Api.Models.Entities;

namespace FargoSecurity.Api.Models.Entities
{
    public class SecurityDbContext:DbContext
    {
        public SecurityDbContext() { }

        public SecurityDbContext(DbContextOptions options)
            :base(options) {}

        public static readonly Microsoft.Extensions.Logging.LoggerFactory MyLoggerFactory =
            new Microsoft.Extensions.Logging.LoggerFactory(new[] {
                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
            });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<System> Systems { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentItem> AssignmentItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfiguration(new SystemConfiguration());
           modelBuilder.ApplyConfiguration(new ModuleConfiguration());
           modelBuilder.ApplyConfiguration(new OptionConfiguration());
           modelBuilder.ApplyConfiguration(new UserConfiguration());
           modelBuilder.ApplyConfiguration(new RolConfiguration());
           modelBuilder.ApplyConfiguration(new ProfileConfiguration());
           modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
           modelBuilder.ApplyConfiguration(new AssignmentItemConfiguration());
           
           base.OnModelCreating(modelBuilder);
        }

        public DbSet<FargoSecurity.Api.Models.Entities.Token> Token { get; set; }
    }
}
