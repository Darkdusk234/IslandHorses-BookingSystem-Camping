using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace Camping_BookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly CampingDbContext _context;

        public BookingRepository(CampingDbContext context)
        {
            _context = context;
        }

        /*-------------------------------------------------------------*/
        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public void Delete(Booking booking)
        {
            _context.Bookings.Remove(booking);
        }

        public  async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(c => c.Customer)
                .Include(b => b.CampSpot)
                .ThenInclude(cs => cs.CampSite)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId)
        {
            return await _context.Bookings
                .Where(b => b.CustomerId == customerId)
                .Include(c => c.Customer)
                .Include(b => b.CampSpot)
                .ThenInclude(cs => cs.CampSite)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
        }
    }
}
