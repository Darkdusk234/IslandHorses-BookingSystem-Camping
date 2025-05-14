using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Repositories
{
    public interface ICampSpotRepository
    {
        public Task<ICollection<CampSpot>> GetAll();
        public Task<CampSpot> GetCampSpotById();
        public Task<ICollection<CampSpot>> GetByCampSiteId();
        public Task Create();
        public Task Update();
        public Task Delete();
    }
}
