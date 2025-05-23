using System.ComponentModel.DataAnnotations;

namespace BookingSystem_ClassLibrary.Models.DTOs.CustomerDTOs;

public class CreateCustomerDto
{
    [Required]
    [StringLength(25)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(30)]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    [Required]
    [StringLength(255)]
    public string StreetAddress { get; set; } = string.Empty;
    [Required]
    [StringLength(255)]
    public string ZipCode { get; set; } = string.Empty;
    [Required]
    [StringLength(255)]
    public string City { get; set; } = string.Empty;
}