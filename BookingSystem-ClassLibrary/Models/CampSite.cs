namespace BookingSystem_ClassLibrary.Models
{
    public class CampSite
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Adress { get; set; } = null!;

        //Navigational properties
        public ICollection<CampSpot> CampSpots { get; set; } = new List<CampSpot>();
    }
}
