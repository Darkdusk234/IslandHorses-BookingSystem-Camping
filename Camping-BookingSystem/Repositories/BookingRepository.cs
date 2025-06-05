using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;
using Microsoft.EntityFrameworkCore;

namespace Camping_BookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly CampingDbContext _context;

        public BookingRepository(CampingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCampSpotAndDate(int campSpotId, DateTime startDate, DateTime endDate)
        {
            return await _context.Bookings
                .Where(b => 
                b.CampSpot.Id == campSpotId &&
                b.StartDate >= startDate &&
                b.StartDate <= endDate)
                .ToListAsync();
        }

        // Method to get all booking details by camp site id (Camp Owner)
        public async Task<IEnumerable<BookingDetailsResponse>> GetBookingDetailsByCampSiteIdAsync(int campSiteId)
        {
            return await _context.Bookings
                .Where(b => b.CampSpot.CampSite.Id == campSiteId)
                .Select(b => new BookingDetailsResponse
                {
                    BookingId = b.Id,
                    StartDate = b.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = b.EndDate.ToString("yyyy-MM-dd"),
                    NumberOfPeople = b.NumberOfPeople,
                    Parking = b.Parking,
                    Wifi = b.Wifi,
                    Status = b.Status.ToString(),
                    CustomerId = b.CustomerId,
                    CustomerName = b.Customer.FirstName + " " + b.Customer.LastName,
                    CampSiteName = b.CampSpot.CampSite.Name,
                    CampSpotType = b.CampSpot.SpotType.Name,

                    NumberOfNights = (b.EndDate - b.StartDate).Days,

                    TotalPrice =
                            (b.CampSpot.SpotType.Price * (b.EndDate - b.StartDate).Days)
                            + (b.Wifi ? 25 * (b.EndDate - b.StartDate).Days : 0)
                            + (b.Parking ? 50 * (b.EndDate - b.StartDate).Days : 0)
                })
                .ToListAsync();
        }

        // Method to get booking details by customer id (Receptionist)
        public async Task<IEnumerable<BookingDetailsResponse>> GetBookingDetailsByCustomerIdAsync(int customerId)
        {
            return await _context.Bookings
                .Where(b => b.CustomerId == customerId)
                .Select(b => new BookingDetailsResponse
                {
                    BookingId = b.Id,
                    StartDate = b.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = b.EndDate.ToString("yyyy-MM-dd"),
                    NumberOfPeople = b.NumberOfPeople,
                    Parking = b.Parking,
                    Wifi = b.Wifi,
                    Status = b.Status.ToString(),
                    CustomerId = b.CustomerId,
                    CustomerName = b.Customer.FirstName + " " + b.Customer.LastName,
                    CampSiteName = b.CampSpot.CampSite.Name,
                    CampSpotType = b.CampSpot.SpotType.Name,

                    NumberOfNights = (b.EndDate - b.StartDate).Days,

                    TotalPrice =
                                (b.CampSpot.SpotType.Price * (b.EndDate - b.StartDate).Days)
                                + (b.Wifi ? 25 * (b.EndDate - b.StartDate).Days : 0)
                                + (b.Parking ? 50 * (b.EndDate - b.StartDate).Days : 0)
                })
                .ToListAsync();
        }

        // Method to get booking details by id (Receptionist)
        public async Task<BookingDetailsResponse?> GetBookingDetailsByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Where(b => b.Id == bookingId)
                .Select(b => new BookingDetailsResponse
                {
                    BookingId = b.Id,
                    StartDate = b.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = b.EndDate.ToString("yyyy-MM-dd"),
                    NumberOfPeople = b.NumberOfPeople,
                    Parking = b.Parking,
                    Wifi = b.Wifi,
                    Status = b.Status.ToString(),
                    CustomerId = b.CustomerId,
                    CustomerName = b.Customer.FirstName + " " + b.Customer.LastName,
                    CampSiteName = b.CampSpot.CampSite.Name,
                    CampSpotType = b.CampSpot.SpotType.Name,

                    NumberOfNights = (b.EndDate - b.StartDate).Days,

                    TotalPrice =
                        (b.CampSpot.SpotType.Price * (b.EndDate - b.StartDate).Days)
                        + (b.Wifi ? 25 * (b.EndDate - b.StartDate).Days : 0)
                        + (b.Parking ? 50 * (b.EndDate - b.StartDate).Days : 0)
                })
                .FirstOrDefaultAsync();
        }

        /*------------------------------------BASIC CRUD-----------------------------------------*/
        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public void Delete(Booking booking)
        {
            _context.Bookings.Remove(booking);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings.ToListAsync();
        }
        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
        }
    }
}
