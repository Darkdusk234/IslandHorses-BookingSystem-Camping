using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookingSystem_ClassLibrary.Models
{
    public class CampSite
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Adress { get; set; } = string.Empty;

        //Navigational properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<CampSpot>? CampSpots { get; set; }
    }
}
