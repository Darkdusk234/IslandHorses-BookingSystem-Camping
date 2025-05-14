using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookingSystem_ClassLibrary.Models
{
    public class Customer
    {
        public int Id { get; set; }
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

        //Navigational Properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Booking>? Bookings { get; set; }
    }
}
