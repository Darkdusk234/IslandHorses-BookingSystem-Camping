using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs
{
    public class AvailableCampSpotResponse
    {
        public int CampSpotId { get; set; }
        public string CampSpotType { get; set; } = string.Empty;
        public string CampSiteName { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
    }
}
