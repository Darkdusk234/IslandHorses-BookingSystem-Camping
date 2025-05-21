using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookingSystem_ClassLibrary.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CampSpotId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int NumberOfPeople { get; set; }

        public bool Parking { get; set; } = false;

        public bool Wifi { get; set; } = false;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        //Navigational Properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Customer? Customer { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CampSpot? CampSpot { get; set; }
    }
}
