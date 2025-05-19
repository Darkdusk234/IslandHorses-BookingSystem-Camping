using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Repositories
{
    public class ISpotTypeRepository
    {
        //Default methods for CRUD operations
        Task<IEnumerable<SpotType>> GetAllAsync();
        Task<SpotType?> GetByIdAsync(int id);
        Task AddAsync(SpotType spotType);
        void Update(SpotType spotType);
        void Delete(SpotType spotType);
        Task SaveAsync();

    }
}
