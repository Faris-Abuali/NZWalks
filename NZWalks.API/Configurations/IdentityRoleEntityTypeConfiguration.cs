using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NZWalks.API.Configurations
{
    public class IdentityRoleEntityTypeConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            var readerRoleId = "ab788e43-77b5-426a-b9b7-e4d13a0a025c";
            var writerRoleId = "6676f9b7-62f4-4d22-8d48-9c1195c64dfd";

            // Seed data for Roles table
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId, // a random value that should change whenever a role is persisted to the data store
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId, // a random value that should change whenever a role is persisted to the data store
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                },
            };

            builder.HasData(roles);
        }
    }
}
