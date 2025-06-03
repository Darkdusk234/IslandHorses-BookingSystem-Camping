using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models.DTOs.SpotTypeDTOs
{
    public class CreateSpotTypeRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int MaxPersonLimit { get; set; }

        //public ICollection<CampSpot>? CampSpots { get; set; }
        //public ICollection<Booking>? Bookings { get; set; }
    }
}
