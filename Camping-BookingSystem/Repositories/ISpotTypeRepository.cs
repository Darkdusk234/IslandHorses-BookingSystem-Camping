using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Repositories
{
    public interface ISpotTypeRepository
    {
        //Default methods for CRUD operations
        Task<IEnumerable<SpotType>> GetAllAsync();
        Task<SpotType?> GetByIdAsync(int id);
        Task AddAsync(SpotType spotType);
        Task UpdateAsync(SpotType spotType);
        Task DeleteAsync(SpotType spotType);
        Task SaveAsync();

    }
}
