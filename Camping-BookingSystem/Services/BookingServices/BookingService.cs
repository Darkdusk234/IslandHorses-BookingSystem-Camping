using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;
using Camping_BookingSystem.Mapping;
using Camping_BookingSystem.Repositories;

namespace Camping_BookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICampSpotRepository _campSpotRepository;
        private readonly ICustomerRepository _customerRepository;
        public BookingService(IBookingRepository bookingRepository, ICampSpotRepository campSpotRepository, ICustomerRepository customerRepository)
        {
            _bookingRepository = bookingRepository;
            _campSpotRepository = campSpotRepository;
            _customerRepository = customerRepository;
        }

        // Method to cancel a booking (Guest)
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

        // Method to create a booking (Guest)
        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveAsync();
            return booking;
        }

        // Method to create a booking and add a customer (Receptionist)
        public async Task<BookingDetailsResponse> CreateBookingWithCustomerAsync(CreateBookingAndCustomer request)
        {
            if (request.StartDate.Date < DateTime.Today) 
            {
                throw new ArgumentException("Start date cannot be in the past.");
            }

            if (request.EndDate.Date <= request.StartDate) 
            {
                throw new ArgumentException("End date must be after start date.");
            }

            var campSpot = await _campSpotRepository.GetCampSpotById(request.CampSpotId);
            if(campSpot == null)
            {
                throw new ArgumentException("Camp spot not found.");
            }
            if(campSpot.SpotType.MaxPersonLimit < request.NumberOfPeople)
            {
                throw new ArgumentException("Camp spot can not accommodate the number of people.");
            }

            var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(request.Email);
            Customer customer;
            if (existingCustomer != null)
            {
                customer = existingCustomer;
            }
            else
                customer = new Customer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    StreetAddress = request.StreetAddress,
                    ZipCode = request.ZipCode,
                    City = request.City
                };

            var booking = new Booking
            {
                Customer = customer,
                CampSpotId = request.CampSpotId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                NumberOfPeople = request.NumberOfPeople,
                Parking = request.Parking,
                Wifi = request.Wifi,
                Status = BookingStatus.Pending
                
            };

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveAsync();

            var response = await _bookingRepository.GetBookingDetailsByIdAsync(booking.Id);

            return response;
        }

        // Method to delete a booking (Camp Owner)
        public async Task DeleteBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking != null)
            {
                _bookingRepository.Delete(booking);
                await _bookingRepository.SaveAsync();
            }
        }

        
        // Method to get all bookings by customer id (Receptionist)
        public async Task<IEnumerable<BookingDetailsResponse>> GetBookingDetailsByCustomerIdAsync(int customerId)
        {
            return await _bookingRepository.GetBookingDetailsByCustomerIdAsync(customerId);
        }

        // Method to get booking details by id (Receptionist)
        public async Task<BookingDetailsResponse?> GetBookingDetailsByIdAsync(int id)
        {
            return await _bookingRepository.GetBookingDetailsByIdAsync(id);
        }
        // Method to get all bookings with details by CampSiteId (Camp Owner)
        public async Task<IEnumerable<BookingDetailsResponse>> GetBookingDetailsByCampSiteIdAsync(int campSiteId)
        {
            return await _bookingRepository.GetBookingDetailsByCampSiteIdAsync(campSiteId);
        }


        // Method to validate different conditions for camp spot availability
        public async Task<(bool IsAvailable, string? Reason)> IsCampSpotAvailableAsync(int campSpotId, DateTime startDate, DateTime endDate, int numberOfPeople)
        {
            if(startDate.Date < DateTime.Today)
            {
                return (false, "Start date cannot be in the past.");
            }
            if (endDate.Date <= startDate)
            {
                return (false, "End date must be after start date.");
            }
            if(startDate.Date > startDate.AddMonths(6))
            {
                return (false, "Booking cannot be made more than 6 months in advance.");
            }

            var campSpot = await _campSpotRepository.GetCampSpotById(campSpotId);
            if(campSpot == null)
            {
                return (false, "Camp spot not found.");
            }
            if (campSpot.SpotType.MaxPersonLimit < numberOfPeople)
            {
                return (false, "Camp spot can not accommodate the number of people.");
            }
           var overlappedBookings = await _bookingRepository.GetBookingsByCampSpotAndDate(campSpotId, startDate, endDate);

            if (overlappedBookings.Any())
            {
                return (false, "Camp spot is not available for the selected dates.");
            }

            return (true, null);
        }

        // Method to update the booking add-ons (Wifi and Parking) (Guest)
        public async Task<(bool Success, string? ErrorMessage)> UpdateBookingAddOnsAsync(int bookingId, UpdateAddonsRequest request)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                return (false, "Booking not found");
            }
            if (booking.Status == BookingStatus.Completed)
            {
                return (false, "Booking can not be updated, it is already completed.");
            }
            if (booking.Status == BookingStatus.Cancelled)
            {
                return (false, "Booking can not be updated, it is already cancelled.");
            }

            booking.Wifi = request.Wifi;
            booking.Parking = request.Parking;

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveAsync();

            return (true, null);
        }
        // Method to update the booking (Receptionist)
        public async Task<(bool Success, string? ErrorMessage)> UpdateBookingAsyn(int bookingId, UpdateBookingRequest request)
        {

            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                return (false, "Booking not found");
            }
            if (booking.Status == BookingStatus.Completed)
            {
                return (false, "Booking can not be updated, it is already completed.");
            }
            if (booking.Status == BookingStatus.Cancelled)
            {
                return (false, "Booking can not be updated, it is already cancelled.");
            }

            var overappedBookings = await _bookingRepository
                .GetBookingsByCampSpotAndDate(
                request.CampSpotId, 
                request.StartDate, 
                request.EndDate);

            if (overappedBookings.Any(b => b.Id != bookingId)) 
            {
                return (false, "Camp spot is not available for the selected dates.");
            }
            if (request.StartDate.Date < DateTime.Today)
            {
                return (false, "Start date cannot be in the past.");
            }
            if (request.EndDate.Date <= request.StartDate)
            {
                return (false, "End date must be after start date.");
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
    }
}
