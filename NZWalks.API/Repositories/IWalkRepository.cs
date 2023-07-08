using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk); // NOTE: Repositories should always deal with Domain Models, not DTOs. So, here it's better to make the param of type `Walk` not of type `AddWalkRequestDto`

        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 20);

        Task<Walk?> GetByIdAsync(Guid id); // could return null since the passed id may not exist

        Task<Walk?> UpdateAsync(Guid id, Walk walk); // could return null since the passed id may not exist

        Task<Walk?> DeleteAsync(Guid id); // could return null since the passed id may not exist
    }

    /**
     * The methods in the interface are not preceded `public async` keywords before each method declaration. This is because the async keyword is not part of the method signature and does not need to be explicitly included in the interface.
     * 
     * When implementing the interface in a class, you would use the async keyword to indicate that you are implementing the asynchronous version of the method. 
     */
}
