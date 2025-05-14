using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Repositories
{
    public interface ICampSpotRepository
    {
        public Task<ICollection<CampSpot>> GetAll();
        public Task<CampSpot> GetCampSpotById(int campSpotId);
        public Task<ICollection<CampSpot>> GetByCampSiteId(int campSiteId);
        public Task Create(CampSpot campSpot);
        public Task Update(CampSpot campSpot);
        public Task Delete(CampSpot campSpot);
    }
}
