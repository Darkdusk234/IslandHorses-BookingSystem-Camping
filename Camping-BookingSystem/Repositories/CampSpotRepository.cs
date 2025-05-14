using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Repositories
{
    public class CampSpotRepository : ICampSpotRepository
    {
        private readonly CampingDbContext _context;

        public CampSpotRepository(CampingDbContext context)
        {
            _context = context;
        }
        public Task Create()
        {
            throw new NotImplementedException();
        }

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<CampSpot>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<CampSpot>> GetByCampSiteId()
        {
            throw new NotImplementedException();
        }

        public Task<CampSpot> GetCampSpotById()
        {
            throw new NotImplementedException();
        }

        public Task Update()
        {
            throw new NotImplementedException();
        }
    }
}
