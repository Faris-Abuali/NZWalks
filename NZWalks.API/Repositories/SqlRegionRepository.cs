using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SqlRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FindAsync(id); // find by PK
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region); // Now the region object will have an Id
            await dbContext.SaveChangesAsync();

            return region; // the returned region object will have an Id property
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FindAsync(id);

            if (existingRegion == null)
            {
                return null;
            }

            // Since `regionDomainModel` is tracked by dbContext, so we can directly update the `regionDomainModel`
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.ImageUrl = region.ImageUrl;

            await dbContext.SaveChangesAsync();

            return existingRegion;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await dbContext.Regions.FindAsync(id);

            if (existingRegion == null)
            {
                return null;
            }

            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();

            /***
               * It's important to note that EF Core performs the actual removal when you call SaveChanges 
               * or SaveChangesAsync. The Remove method only marks the entity as deleted in the change 
               * tracker until the changes are saved.
            */

            return existingRegion;
        }
    }
}
