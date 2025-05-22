using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;

namespace Camping_BookingSystem.Services
{
    public interface IBookingService
    {
        Task<bool> IsCampSpotAvailableAsync(int campSpotId, DateTime startDate, DateTime endDate);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<IEnumerable<BookingDetailsResponse>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<IEnumerable<BookingDetailsResponse>> GetBookingsByCustomerIdAsync(int customerId);
        Task <(bool Success, string? ErrorMessage)> UpdateBookingAsyn(int bookingId, UpdateBookingRequest request);
        Task DeleteBookingAsync(int id);
        Task<(bool Success, string? ErrorMEssage)> CancelBookingAsync(int bookingId);
        Task<Booking> CreateBookingWithCustomerAsync(CreateBookingAndCustomer request);
    }
}
