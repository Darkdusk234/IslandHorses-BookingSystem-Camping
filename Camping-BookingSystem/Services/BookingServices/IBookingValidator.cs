using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;

namespace Camping_BookingSystem.Services.BookingServices
{
    public interface IBookingValidator
    {
        Task<(bool IsValid, string? ErrorMessage)> ValidateCreateAsync(CreateBookingAndCustomer request);
        Task<(bool IsValid, string? ErrorMessage)> ValidateUpdateAsync(UpdateBookingRequest request);
        Task<(bool IsValid, string? ErrorMessage)> ValidateDeleteAsync(int bookingId);
    }
}
