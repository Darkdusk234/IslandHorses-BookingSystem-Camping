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
                BookingId = booking.Id,
                CampSiteName = booking.CampSpot?.CampSite?.Name ?? string.Empty,
                CampSpotType = booking.CampSpot != null
                ? $"{booking.CampSpot?.SpotType?.Name}"
                : string.Empty,
                StartDate = booking.StartDate.ToString("yyyy-MM-dd"),
                EndDate = booking.EndDate.ToString("yyyy-MM-dd"),
                NumberOfPeople = booking.NumberOfPeople,
                Parking = booking.Parking,
                Wifi = booking.Wifi,
                Status = booking.Status.ToString(),
                CustomerId = booking.CustomerId,
                CustomerName = booking.Customer !=null
                ? $"{booking.Customer.FirstName} {booking.Customer.LastName}"
                : string.Empty

            };
        }
    }
}
