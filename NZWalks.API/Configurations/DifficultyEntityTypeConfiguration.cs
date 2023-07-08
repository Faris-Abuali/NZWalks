using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NZWalks.API.Models.Domain;
using System.Reflection.Emit;

namespace NZWalks.API.Configurations
{
    public class DifficultyEntityTypeConfiguration : IEntityTypeConfiguration<Difficulty>
    {
        public void Configure(EntityTypeBuilder<Difficulty> builder)
        {
            // Seed data for Difficulties table
            // Easy, Medium, Hard
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    //Id = Guid.NewGuid(), //This will change the value everytime we run EFCore migrations :)
                    Id = Guid.Parse("cede998f-110b-4206-babd-97c82b516642"), //This will change the value everytime we run EFCore migrations :)
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("aa374cde-b1a7-44a0-b03a-c683b16556ec"), //This will change the value everytime we run EFCore migrations :)
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("b7567802-b688-4355-809e-fd344ef9e929"), //This will change the value everytime we run EFCore migrations :)
                    Name = "Hard"
                },
            };

            builder.HasData(difficulties);
        }
    }
}
