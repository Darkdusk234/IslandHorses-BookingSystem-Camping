using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Repositories
{
    public interface IBookingRepository
    {
        
        //Default methods for CRUD operations
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task AddAsync(Booking booking);
        void Update(Booking booking);
        void Delete(Booking booking);
        Task SaveAsync();

        //Custom methods
        Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId);
    }
}
