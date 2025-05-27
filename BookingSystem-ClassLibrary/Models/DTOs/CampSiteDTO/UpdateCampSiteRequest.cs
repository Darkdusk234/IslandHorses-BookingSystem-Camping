using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem_ClassLibrary.Models.DTOs.CampSiteDTO
{
    public class UpdateCampSiteRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Adress { get; set; } = string.Empty;
        
        //Navigational properties
        //public ICollection<CreateCampSpotRequest>? CampSpots { get; set; }
    }
}
