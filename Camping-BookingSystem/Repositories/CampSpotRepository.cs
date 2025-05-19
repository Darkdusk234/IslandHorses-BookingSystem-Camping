using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace Camping_BookingSystem.Repositories
{
    public class CampSpotRepository : ICampSpotRepository
    {
        private readonly CampingDbContext _context;

        public CampSpotRepository(CampingDbContext context)
        {
            _context = context;
        }
        public async Task Create(CampSpot campSpot)
        {
            await _context.CampSpots.AddAsync(campSpot);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(CampSpot campSpot)
        {
            _context.CampSpots.Remove(campSpot);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<CampSpot>> GetAll()
        {
            return await _context.CampSpots.ToListAsync();
        }

        public async Task<ICollection<CampSpot>> GetCampSpotsByCampSiteId(int campSiteId)
        {
            return await _context.CampSpots.Where(cs => cs.CampSiteId == campSiteId).ToListAsync();
        }

        public async Task<CampSpot?> GetCampSpotById(int campSpotId)
        {
            return await _context.CampSpots.FindAsync(campSpotId);
        }

        public async Task Update(CampSpot campSpot)
        {
            _context.CampSpots.Update(campSpot);
            await _context.SaveChangesAsync();
        }
    }
}
