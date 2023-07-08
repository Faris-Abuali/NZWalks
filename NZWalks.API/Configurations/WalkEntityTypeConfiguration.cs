using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Configurations
{
    public class WalkEntityTypeConfiguration : IEntityTypeConfiguration<Walk>
    {
        public void Configure(EntityTypeBuilder<Walk> builder)
        {
            // Seed data for Walks table
            var walks = new List<Walk>()
            {
                new Walk
                {
                    Id = Guid.Parse("38cf7293-a3f3-42cb-8b7d-08db79c1cb12"),
                    Name = "Mount Victoria Lookout Walk",
                    Description = "This is a description",
                    LengthInKm = 8.83,
                    ImageUrl = "image.jpeg",
                    DifficultyId = Guid.Parse("aa374cde-b1a7-44a0-b03a-c683b16556ec"),
                    RegionId = Guid.Parse("53ddd3ff-bc43-416a-9036-c8f9aa8470bf"),
                }
            };

            builder.HasData(walks);
        }
    }
}
