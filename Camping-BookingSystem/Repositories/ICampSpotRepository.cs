using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;


namespace Camping_BookingSystem.Repositories
{
    public interface ICampSpotRepository
    {
        public Task<ICollection<CampSpot>> GetAll();
        public Task<CampSpot?> GetCampSpotById(int campSpotId);
        public Task<ICollection<CampSpot>> GetCampSpotsByCampSiteId(int campSiteId);
        public Task<List<CampSpot>> GetAvailableCampSpotsAsync(DateTime startDate, DateTime endDate, int typeId/*, int nrGuests*/);
        public Task<(bool, string?)> Create(CampSpot campSpot);
        public Task<(bool, string?)> Update(CampSpot campSpot);
        public Task<(bool, string?)> Delete(CampSpot campSpot);
        
         
        Task<IEnumerable<CampSpot>> SearchAvailableSpots(SearchAvailableSpotsDto searchDto);

    }
}
