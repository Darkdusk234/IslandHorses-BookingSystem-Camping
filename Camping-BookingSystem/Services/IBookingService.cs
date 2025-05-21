using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;

namespace Camping_BookingSystem.Services
{
    public interface IBookingService
    {
        Task<bool> IsCampSpotAvailableAsync(int campSpotId, DateTime startDate, DateTime endDate);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<IEnumerable<BookingDetailsResponse>> GetBookingsByCustomerIdAsync(int customerId);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int id);
    }
}
