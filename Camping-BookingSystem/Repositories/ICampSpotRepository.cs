using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;

namespace Camping_BookingSystem.Repositories
{
    public interface ICampSpotRepository
    {
        public Task<ICollection<CampSpot>> GetAll();
        public Task<CampSpot?> GetCampSpotById(int campSpotId);
        public Task<ICollection<CampSpot>> GetCampSpotsByCampSiteId(int campSiteId);
        public Task<List<SpotsBasedOnDatesRequest>> GetAvailableCampSpotsAsync(DateTime startDate, DateTime endDate, int campSiteId);
        public Task Create(CampSpot campSpot);
        public Task Update(CampSpot campSpot);
        public Task Delete(CampSpot campSpot);
        
         
        Task<IEnumerable<CampSpot>> SearchAvailableSpots(SearchAvailableSpotsDto searchDto);
    }
}
