using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace Camping_BookingSystem.Repositories
{
    public class SpotTypeRepository : ISpotTypeRepository
    {
        private readonly CampingDbContext _context;
        public SpotTypeRepository(CampingDbContext context)
        {
            _context = context;
        }
        // Default methods for CRUD operations
        public async Task<IEnumerable<SpotType>> GetAllAsync()
        {
            return await _context.SpotTypes.ToListAsync();
        }
        public async Task<SpotType?> GetByIdAsync(int id)
        {
            return await _context.SpotTypes.FindAsync(id);
        }
        public async Task AddAsync(SpotType spotType)
        {
            await _context.SpotTypes.AddAsync(spotType);
        }
        public void Update(SpotType spotType)
        {
            _context.SpotTypes.Update(spotType);
        }
        public void Delete(SpotType spotType)
        {
            _context.SpotTypes.Remove(spotType);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
