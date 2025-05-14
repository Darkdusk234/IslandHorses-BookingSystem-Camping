using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Repositories;

namespace Camping_BookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }


        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveAsync();
            return booking;
        }

        public Task DeleteBookingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Booking?> GetBookingByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsCampSpotAvailableAsync(int campSpotId, DateTime startDate, DateTime endDate)
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return !bookings.Any(b =>
                b.CampSpotId == campSpotId &&
                b.EndDate >= startDate &&
                b.StartDate <= endDate);
        }

        public Task UpdateBookingAsync(Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}
