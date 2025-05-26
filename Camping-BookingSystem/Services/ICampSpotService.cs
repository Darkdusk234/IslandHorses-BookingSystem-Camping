using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;

namespace Camping_BookingSystem.Services
{
    public interface ICampSpotService
    {
        Task<IEnumerable<CampSpot>> GetAllCampSpotsAsync();
        Task<CampSpot?> GetCampSpotByIdAsync(int id);
        Task<IEnumerable<CampSpot>> GetCampSpotsByCampSiteIdAsync(int campSiteId);
        Task<CampSpot> AddCampSpotAsync(CampSpot campSpot);
        Task DeleteCampSpotAsync(int id);
        Task UpdateCampSpotAsync(CampSpot campSpot);

        // Receptions for searching available spots
        Task<IEnumerable<CampSpot>> SearchAvailableSpotsAsync(SearchAvailableSpotsDto searchDto);
    }
}
