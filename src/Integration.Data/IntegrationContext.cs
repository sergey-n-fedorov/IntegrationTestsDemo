using Integration.Data.Configurations;
using Integration.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IntegrationService.Data
{
    public class IntegrationContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        private readonly string _connectionString;

        public IntegrationContext(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("IntergrationDb")!;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}