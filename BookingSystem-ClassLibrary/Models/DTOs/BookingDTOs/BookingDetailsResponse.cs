using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs
{
    public class BookingDetailsResponse
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CampSiteName { get; set; } = string.Empty;
        public string CampSpotType { get; set; } = string.Empty;
        public bool Parking { get; set; }
        public bool Wifi { get; set; }
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public int NumberOfPeople { get; set; }
        public int NumberOfNights { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
