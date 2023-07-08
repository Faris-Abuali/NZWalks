using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Configurations
{
    public class RegionEntityTypeConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            // Seed data for Regions table
            var regions = new List<Region>()
            {
                new Region()
                {
                    Id = Guid.Parse("53ddd3ff-bc43-416a-9036-c8f9aa8470bf"),
                    Name = "Auckland",
                    Code = "AKL",
                    ImageUrl = "https://images.pexels.com/photos/5342974/pexels-photo-5342974.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2"
                },
                new Region()
                {
                    Id = Guid.Parse("e8380a74-7558-4074-aa9d-813cdfe440bc"),
                    Name = "Northland",
                    Code = "NTL",
                    ImageUrl = null
                },
                new Region()
                {
                    Id = Guid.Parse("34aaf3a6-e25f-4b9b-aaf8-6b2dc4720456"),
                    Name = "Bay of Plenty",
                    Code = "BOP",
                    ImageUrl = null
                },
                new Region()
                {
                    Id = Guid.Parse("d40c77a5-8939-4198-9023-dc4929a6f799"),
                    Name = "Wellington",
                    Code = "WGN",
                    ImageUrl = "https://images.pexels.com/photos/8379417/pexels-photo-8379417.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2"
                },
                new Region()
                {
                    Id = Guid.Parse("3fd2b0b5-3662-4575-923c-07701df2bf82"),
                    Name = "Nelson",
                    Code = "NSN",
                    ImageUrl = "https://photos.smugmug.com/New-Zealand-2016/Nelson-/i-wvD5rSk/0/O/Nelson%20New%20Zealand-4693.jpg"
                },
                new Region()
                {
                    Id = Guid.Parse("4a5f65d7-1f19-4578-889c-3d44d23e8584"),
                    Name = "Southland",
                    Code = "STL",
                },
            };

            builder.HasData(regions);
        }
    }
}
