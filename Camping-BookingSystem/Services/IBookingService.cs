using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;

namespace Camping_BookingSystem.Services
{
    public interface IBookingService
    {
        Task DeleteBookingAsync(int id);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<(bool IsAvailable, string? Reason)> IsCampSpotAvailableAsync(int campSpotId, DateTime startDate, DateTime endDate, int numberOfPeople);
        Task <(bool Success, string? ErrorMessage)> UpdateBookingAsyn(int bookingId, UpdateBookingRequest request);
        Task<(bool Success, string? ErrorMEssage)> CancelBookingAsync(int bookingId);
        Task<(bool Success, string? ErrorMessage)> UpdateBookingAddOnsAsync(int bookingId, UpdateAddonsRequest request);
        Task<BookingDetailsResponse> CreateBookingWithCustomerAsync(CreateBookingAndCustomer request);
        Task<BookingDetailsResponse?> GetBookingDetailsByIdAsync(int id);
        Task<IEnumerable<BookingDetailsResponse>> GetBookingDetailsByCustomerIdAsync(int customerId);
        Task<IEnumerable<BookingDetailsResponse>> GetBookingDetailsByCampSiteIdAsync(int campSiteId);

    }
}
