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
        public async Task Create()
        {
            throw new NotImplementedException();
        }

        public async Task Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<CampSpot>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<CampSpot>> GetByCampSiteId()
        {
            throw new NotImplementedException();
        }

        public async Task<CampSpot> GetCampSpotById()
        {
            throw new NotImplementedException();
        }

        public async Task Update()
        {
            throw new NotImplementedException();
        }
    }
}
