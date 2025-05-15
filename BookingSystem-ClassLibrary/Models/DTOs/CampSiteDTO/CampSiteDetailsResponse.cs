using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models.DTOs
{
    public class CampSiteDetailsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Adress { get; set; } = string.Empty;

        //Navigational properties
        //public ICollection<CampSpotResponse>? CampSpots { get; set; }
    }
}
