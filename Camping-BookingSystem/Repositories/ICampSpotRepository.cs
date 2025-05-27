using System.Runtime.InteropServices.JavaScript;
using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Repositories
{
    public interface ICampSpotRepository
    {
        public Task<ICollection<CampSpot>> GetAll();
        public Task<CampSpot?> GetCampSpotById(int campSpotId);
        public Task<ICollection<CampSpot>> GetCampSpotsByCampSiteId(int campSiteId);
        public Task<List<CampSpot>> GetAvailableCampSpotsAsync(DateTime startDate, DateTime endDate, int typeId/*, int nrGuests*/);
        public Task Create(CampSpot campSpot);
        public Task Update(CampSpot campSpot);
        public Task Delete(CampSpot campSpot);
    }
}
