namespace BookingSystem_ClassLibrary.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CampSpotId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfPeople { get; set; }

        //Navigational Properties
        public Customer? Customer { get; set; }
        public CampSpot? CampSpot { get; set; }
    }
}
