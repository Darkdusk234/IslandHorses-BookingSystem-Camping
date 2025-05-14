namespace BookingSystem_ClassLibrary.Models
{
    public class SpotType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }

        //Navigational Properties
        public ICollection<CampSpot>? CampSpots { get; set; }
    }
}
