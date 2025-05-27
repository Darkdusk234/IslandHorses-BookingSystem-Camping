using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs
{
    public class CreateBookingAndCustomer
    {
        
        [Required, MinLength(1)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MinLength(1)]
        public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress, MinLength(1)]
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        [Required, MinLength(1)]
        public string StreetAddress { get; set; } = string.Empty;
        [Required, MinLength(1)]
        public string ZipCode { get; set; } = string.Empty;    
        [Required, MinLength(1)]
        public string City { get; set; } = string.Empty;
        [Required]
        public int CampSpotId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Range(1, 10, ErrorMessage = "Number of people must be between 1 and 10.")]
        public int NumberOfPeople { get; set; }
        public bool Wifi { get; set; } = false;
        public bool Parking { get; set; } = false;
    }
}
