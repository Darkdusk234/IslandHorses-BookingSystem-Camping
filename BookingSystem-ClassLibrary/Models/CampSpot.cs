using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models
{
    public class CampSpot
    {
        public int Id { get; set; }
        public int CampSiteId { get; set; }
        public int TypeId { get; set; }
        public bool Electricity { get; set; } = false;
        public int MaxPersonLimit { get; set; }

        //Navigational properties
        public CampSite? CampSite { get; set; }
        public Type? Type { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}
