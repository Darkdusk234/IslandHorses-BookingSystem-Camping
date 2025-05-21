using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models
{
    public class CampSpot
    {
        public int Id { get; set; }
        public int CampSiteId { get; set; }
        public int TypeId { get; set; }
        public bool Electricity { get; set; } = false;

        //Navigational properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CampSite? CampSite { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SpotType? SpotType { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Booking>? Bookings { get; set; }
    }
}
