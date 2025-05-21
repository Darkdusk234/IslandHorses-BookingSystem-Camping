using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;

namespace Camping_BookingSystem.Mapping
{
    public static class BookingMapper
    {
        public static Booking ToBooking(this CreateBookingRequest dto)
        {
            return new Booking
            {
                CustomerId = dto.CustomerId,
                CampSpotId = dto.CampSpotId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                NumberOfPeople = dto.NumberOfPeople,
                Wifi = dto.Wifi,
                Parking = dto.Parking,
                Status = BookingStatus.Pending
            };
        }

        public static BookingDetailsResponse ToBookingDetailsResponse(this Booking booking)
        {
            return new BookingDetailsResponse
            {
                Id = booking.Id,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                NumberOfPeople = booking.NumberOfPeople,
                Parking = booking.Parking,
                Wifi = booking.Wifi,
                Status = booking.Status.ToString(),

                CampSiteName = booking.CampSpot?.CampSite?.Name ?? string.Empty,

                CustomerName = booking.Customer !=null
                ? $"{booking.Customer.FirstName} {booking.Customer.LastName}"
                : string.Empty,

                CampSpotName = booking.CampSpot != null
                ? $"{booking.CampSpot.Id}"
                : string.Empty

            };
        }
    }
}
