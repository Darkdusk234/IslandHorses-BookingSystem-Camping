using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;

namespace Camping_BookingSystem.Services
{
    public interface ICampSpotService
    {
        Task<IEnumerable<CampSpot>> GetAllCampSpotsAsync();
        Task<CampSpot?> GetCampSpotByIdAsync(int id);
        Task<(IEnumerable<CampSpot>?, bool campSiteFound)> GetCampSpotsByCampSiteIdAsync(int campSiteId);

        Task<IEnumerable<CampSpot>> GetAvailableSpotsMatchingDates(DateTime startDate, DateTime endDate);
        Task<CampSpot> AddCampSpotAsync(CampSpot campSpot);

        Task<(bool success, string? errorMessage)> DeleteCampSpotAsync(int id);
        Task<(bool success, string? errorMessage)> UpdateCampSpotAsync(int id, CreateCampSpotRequest request);

        // Receptions for searching available spots
        
        // Task<IEnumerable<CampSpot>> SearchAvailableSpotsAsync(SearchAvailableSpotsDto searchDto);    // Old one
        Task<SearchResult<CampSpot>> SearchAvailableSpotsAsync(SearchAvailableSpotsDto searchDto);  // updated one 

    }
}
