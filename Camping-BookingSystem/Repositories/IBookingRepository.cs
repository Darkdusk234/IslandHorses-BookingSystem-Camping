using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;

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
        Task<IEnumerable<Booking>> GetBookingsByCampSpotAndDate(int campSpotId, DateTime startDate, DateTime endDate);
    }
}
