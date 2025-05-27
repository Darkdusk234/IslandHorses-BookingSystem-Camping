using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Services
{
    public interface ISpotTypeService
    {
        Task<IEnumerable<SpotType>> GetAllSpotTypesAsync();
        Task<SpotType?> GetSpotTypeByIdAsync(int id);
        Task<SpotType> CreateSpotTypeAsync(SpotType spotType);
        Task UpdateSpotTypeAsync(SpotType spotType);
        Task DeleteSpotTypeAsync(int id);


    }
}
