using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs
{
    public class UpdateBookingRequest
    {

        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int CampSpotId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Range(1, 10, ErrorMessage = "Number of people must be between 1 and 10.")]
        public int NumberOfPeople { get; set; }
        public bool Parking { get; set; }
        public bool Wifi { get; set; }
        public BookingStatus Status { get; set; }

    }
}
