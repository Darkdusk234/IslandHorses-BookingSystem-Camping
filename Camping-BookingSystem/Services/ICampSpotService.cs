using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;

namespace Camping_BookingSystem.Services
{
    public interface ICampSpotService
    {
        Task<IEnumerable<CampSpot>> GetAllCampSpotsAsync();
        Task<CampSpot?> GetCampSpotByIdAsync(int id);
        Task<(IEnumerable<CampSpot>?, bool campSiteFound)> GetCampSpotsByCampSiteIdAsync(int campSiteId);

        Task<IEnumerable<CampSpot>> GetAvailableSpotsMatchingNeeds(DateTime startDate, DateTime endDate,
            int typeId /*, int nrGuests*/);
        Task<CampSpot> AddCampSpotAsync(CampSpot campSpot);
        Task DeleteCampSpotAsync(int id);
        Task<(bool success, string? errorMessage)> UpdateCampSpotAsync(int id, CreateCampSpotRequest request);
    }
}
