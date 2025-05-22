using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;
using Camping_BookingSystem.Mapping;
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

        public async Task<(bool Success, string? ErrorMEssage)> CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                return (false, "Booking not found");
            }
            if (booking.Status == BookingStatus.Cancelled)
            {
                return (false, "Booking is already cancelled");
            }
            if (booking.Status == BookingStatus.Completed)
            {
                return (false, "Booking is already completed");
            }

            booking.Status = BookingStatus.Cancelled;
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveAsync();

            return (true, null);

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

        public async Task<IEnumerable<BookingDetailsResponse>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();

            return bookings.Select(b =>
            {
                var dto = b.ToBookingDetailsResponse();
                dto.NumberOfNights = CalculateTotalNights(b);
                dto.TotalPrice = CalculateTotalPrice(b);
                return dto;
            });
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<BookingDetailsResponse>> GetBookingsByCustomerIdAsync(int customerId)
        {
            var bookings = await _bookingRepository.GetBookingsByCustomerIdAsync(customerId);
            

            var result = bookings.Select(b =>
            {
                var dto = b.ToBookingDetailsResponse();
                dto.NumberOfNights = CalculateTotalNights(b);
                dto.TotalPrice = CalculateTotalPrice(b);
                return dto;
            });

            return result;

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

        public async Task<(bool Success, string? ErrorMessage)> UpdateBookingAsyn(int bookingId, UpdateBookingRequest request)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                return (false, "Booking not found");
            }
            if (booking.Status == BookingStatus.Completed)
            {
                return (false, "Booking is already completed");
            }
            booking.CampSpotId = request.CampSpotId;
            booking.StartDate = request.StartDate;
            booking.EndDate = request.EndDate;
            booking.NumberOfPeople = request.NumberOfPeople;
            booking.Parking = request.Parking;
            booking.Wifi = request.Wifi;
            booking.Status = request.Status;

            await _bookingRepository.SaveAsync();

            return (true, null);

        }

        private int CalculateTotalNights(Booking booking)
        {
            return (booking.EndDate - booking.StartDate).Days;
        }

        private decimal CalculateTotalPrice(Booking booking)
        {
            if (booking.CampSpot?.SpotType == null)
            {
                Console.WriteLine($"SpotType is missing for booking {booking.Id}");
                return 0;
            }
            var basePrice = booking.CampSpot.SpotType.Price;
            Console.WriteLine($"Booking: {booking.Id} - SpotType: {booking.CampSpot.SpotType.Name}, Price per night: {basePrice}");

            int totalNights = CalculateTotalNights(booking);
            decimal extra = 0;

            if(booking.Wifi) extra += 25* totalNights;
            if(booking.Parking) extra += 50 * totalNights;


            return (basePrice * totalNights) + extra;
        }


    }
}
