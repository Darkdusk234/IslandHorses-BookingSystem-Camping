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

        public async Task DeleteBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking != null)
            {
                _bookingRepository.Delete(booking);
                await _bookingRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId)
        {
            return await _bookingRepository.GetBookingsByCustomerIdAsync(customerId);
        }
        // bool to check if the camp spot is available by comparing the start and end dates of the booking with the existing bookings
        public async Task<bool> IsCampSpotAvailableAsync(int campSpotId, DateTime startDate, DateTime endDate)
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return !bookings.Any(b =>
                b.CampSpotId == campSpotId &&
                b.EndDate >= startDate &&
                b.StartDate <= endDate);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveAsync();
        }

        private decimal CalculateTotalPrice(Booking booking)
        {
            int totalDays = (booking.EndDate - booking.StartDate).Days;
            decimal basePrice = booking.CampSpot?.SpotType?.Price ?? 0;
            decimal extra = 0;

            if(booking.Wifi) extra += 25* totalDays;
            if(booking.Parking) extra += 50 * totalDays;


            return (basePrice * totalDays) + extra;
        }
    }
}
