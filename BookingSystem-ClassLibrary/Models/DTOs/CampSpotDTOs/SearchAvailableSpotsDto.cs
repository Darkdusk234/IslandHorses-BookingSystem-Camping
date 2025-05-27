using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs
{
    public class SearchAvailableSpotsDto
    {
        public int CampSiteId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SpotTypeId { get; set; } // Type of the spot (Tent, RV, Cabin)
        public int NumberOfPeople { get; set; }
        public bool? RequiresElectricity { get; set; } = false; // Optional parameter to filter spots with electricity
    }
}
