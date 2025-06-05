using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Services
{
    public interface IBookingService
    {
        Task<bool> IsCampSpotAvailableAsync(int campSpotId, DateTime startDate, DateTime endDate);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int id);
    }
}
