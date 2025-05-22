using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;
using Camping_BookingSystem.Mapping;
using Camping_BookingSystem.Repositories;
using Camping_BookingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Camping_BookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var response = await _bookingService.GetAllBookingsAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            var response = booking.ToBookingDetailsResponse();
            return Ok(response);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetBookingsByCustomerId(int customerId)
        {
            var bookings = await _bookingService.GetBookingsByCustomerIdAsync(customerId);
            
            if(!bookings.Any())
            {
                return NotFound("No bookings found for the specified customer.");
            }

            return Ok(bookings);

        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            var isAvailable = await _bookingService
                .IsCampSpotAvailableAsync(request.CampSpotId, request.StartDate, request.EndDate);

            if (!isAvailable)
            {
                return BadRequest("Camp spot is not available for the selected dates.");
            }

            var booking = request.ToBooking();
            var createdBooking = await _bookingService.CreateBookingAsync(booking);

            var response = createdBooking.ToBookingDetailsResponse();
            return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, response);
        }
        
        [HttpPut("{id}/UpdateBooking")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Enum.IsDefined(typeof(BookingStatus), request.Status ))
            {
                return BadRequest("Invalid booking status.");
            }
            var (success, errorMessage) = await _bookingService.UpdateBookingAsyn(id, request);

            if (!success)
            {
                return BadRequest(errorMessage);
            }

            return NoContent();
        }

        [HttpPatch("{id}/CancelBooking")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var (success, errorMessage) = await _bookingService.CancelBookingAsync(id);
            if (!success)
            {
                return BadRequest(errorMessage);
            }

            return Ok($"Booking with ID {id} has been cancelled. See u next time.");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var existingBooking = await _bookingService.GetBookingByIdAsync(id);
            if (existingBooking == null)
            {
                return NotFound();
            }
            await _bookingService.DeleteBookingAsync(id);
            return NoContent();
        }
    }
}
