using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookingSystem_ClassLibrary.Models
{
    public class SpotType
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Precision(8, 2)]
        public decimal Price { get; set; }

        //Navigational Properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<CampSpot>? CampSpots { get; set; }
    }
}
