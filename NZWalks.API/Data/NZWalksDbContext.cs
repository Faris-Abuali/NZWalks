using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using NZWalks.API.Configurations;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Walk> Walks { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FluentAPI configuration for Entities:
            new RegionEntityTypeConfiguration().Configure(modelBuilder.Entity<Region>());
            new DifficultyEntityTypeConfiguration().Configure(modelBuilder.Entity<Difficulty>());
            new WalkEntityTypeConfiguration().Configure(modelBuilder.Entity<Walk>());
        }
    }
}
