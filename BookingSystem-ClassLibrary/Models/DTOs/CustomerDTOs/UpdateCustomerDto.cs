using System.ComponentModel.DataAnnotations;

namespace BookingSystem_ClassLibrary.Models.DTOs.CustomerDTOs;

public record UpdateCustomerDto
{
    [Required] public string FirstName { get; init; } = string.Empty;
    [Required] [StringLength(30)] public string LastName { get; init; } = string.Empty;
    [Required] [StringLength(255)] [EmailAddress] public string Email { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    [Required] [StringLength(255)] public string StreetAddress { get; init; } = string.Empty;
    [Required] [StringLength(255)] public string ZipCode { get; init; } = string.Empty;
    [Required] [StringLength(255)] public string City { get; init; } = string.Empty;
}