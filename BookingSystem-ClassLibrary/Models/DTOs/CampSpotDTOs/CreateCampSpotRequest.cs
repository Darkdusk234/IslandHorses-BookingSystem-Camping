using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs
{
    internal class CreateCampSpotRequest
    {
        [Required]
        public int CampSiteId { get; set; }
        [Required]
        public int TypeId { get; set; }
        public bool Electricity { get; set; }

    }
}
