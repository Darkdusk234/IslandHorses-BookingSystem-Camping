using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Services
{
    public interface ICampSpotService
    {
        Task<IEnumerable<CampSpot>> GetAllCampSpotsAsync();
        Task<CampSpot?> GetCampSpotByIdAsync(int id);
        Task<IEnumerable<CampSpot>> GetCampSpotsByCampSiteIdAsync(int campSiteId);
        Task CreateCampSpotAsync(CampSpot campSpot);
        Task DeleteCampSpotAsync(int id);
        Task UpdateCampSpotAsync(CampSpot campSpot);
    }
}
