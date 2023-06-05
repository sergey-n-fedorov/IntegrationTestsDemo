using Integration.Data.Configurations;
using Integration.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Integration.Data
{
    public class IntegrationContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        private readonly IntegrationContextConfiguration _contextConfiguration;

        public IntegrationContext(IntegrationContextConfiguration contextConfiguration)
        {
            _contextConfiguration = contextConfiguration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_contextConfiguration.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}