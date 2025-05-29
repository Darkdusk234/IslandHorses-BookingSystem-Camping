using Azure.Core;
using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;
using Camping_BookingSystem.Repositories;

namespace Camping_BookingSystem.Services.BookingServices
{
    public class BookingValidator : IBookingValidator
    {
        private readonly ICampSpotRepository _campSpotRepository;
        private readonly IBookingRepository _bookingRepository;

        public BookingValidator(ICampSpotRepository campSpotRepository, IBookingRepository bookingRepository)
        {
            _campSpotRepository = campSpotRepository;
            _bookingRepository = bookingRepository;
        }


        public async Task<(bool IsValid, string? ErrorMessage)> ValidateCreateAsync(CreateBookingAndCustomer request)
        {
            if (request.StartDate.Date < DateTime.Today)
                return (false, "Start date cannot be in the past.");

            if (request.EndDate.Date <= request.StartDate)
                return (false, "End date must be after start date.");

            var campSpot = await _campSpotRepository.GetCampSpotById(request.CampSpotId);
            if (campSpot == null)
                return (false, "Camp spot not found.");

            if (request.NumberOfPeople > campSpot.SpotType.MaxPersonLimit)
                return (false, $"Too many people for this camp spot. Max allowed is {campSpot.SpotType.MaxPersonLimit}.");

            return (true, null);
        }

        public async Task<(bool IsValid, string? ErrorMessage)> ValidateUpdateAsync(UpdateBookingRequest request)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.CustomerId);
            if (booking == null)
                return (false, "Booking not found.");

            if (request.StartDate.Date < DateTime.Today)
                return (false, "Start date cannot be in the past.");

            if (request.EndDate.Date <= request.StartDate)
                return (false, "End date must be after start date.");

            var campSpot = await _campSpotRepository.GetCampSpotById(request.CampSpotId);
            if (campSpot == null)
                return (false, "Camp spot not found.");

            if (request.NumberOfPeople > campSpot.SpotType.MaxPersonLimit)
                return (false, $"Too many people for this camp spot. Max allowed is {campSpot.SpotType.MaxPersonLimit}.");

            return (true, null);
        }

        public async Task<(bool IsValid, string? ErrorMessage)> ValidateDeleteAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                return (false, "Booking not found.");
            }
            if(booking.Status == BookingStatus.Completed)
            {
                return (false, "Only pending bookings can be deleted.");
            }
            if(booking.Status == BookingStatus.Cancelled)
            {
                return (false, "This booking has already been cancelled.");
            }
            
            return (true, null);
        }
    }
}
