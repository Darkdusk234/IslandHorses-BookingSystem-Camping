using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;

namespace BookingSystem_ClassLibrary.Models.DTOs.CustomerDTOs;

public record CustomerResponse
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public string StreetAddress { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
        
    public ICollection<BookingDetailsResponse>? Bookings { get; init; }
}