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

        /*------------------------------------------------ CAMP OWNER -----------------------------------------------------*/
        
        [Tags("Camp Owner")]
        [HttpGet("campsite/{campSiteId}")]
        public async Task<IActionResult> GetBookingsByCampSiteId(int campSiteId)
        {
            var bookings = await _bookingService.GetBookingDetailsByCampSiteIdAsync(campSiteId);

            if (!bookings.Any())
            {
                return NotFound($"No bookings found for CampSite ID {campSiteId}");
            }

            return Ok(bookings);
        }

        [Tags("Camp Owner")]
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
        /*----------------------------------------------- RECEPTIONIST ----------------------------------------------------*/

        [Tags("Receptionist")]
        [HttpPost("CreateBookingWithCustomer")]
        public async Task<IActionResult> CreateBookingWithCustomer([FromBody] CreateBookingAndCustomer request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (isAvailable, reason) = await _bookingService
                .IsCampSpotAvailableAsync(request.CampSpotId, request.StartDate, request.EndDate, request.NumberOfPeople);
            if (!isAvailable)
            {
                return BadRequest(reason);
            }

            try
            {
                var response = await _bookingService.CreateBookingWithCustomerAsync(request);
                return CreatedAtAction(nameof(GetBookingById), new { id = response.BookingId }, response);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Tags("Receptionist")]
        [HttpPut("UpdateBooking/{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Enum.IsDefined(typeof(BookingStatus), request.Status))
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

        [Tags("Receptionist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var response = await _bookingService.GetBookingDetailsByIdAsync(id);
            if (response == null)
            {
                return NotFound($"No booking found with ID {id}");
            }

            return Ok(response);
        }


        [Tags("Receptionist")]
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetBookingsByCustomerId(int customerId)
        {
            var bookings = await _bookingService.GetBookingDetailsByCustomerIdAsync(customerId);

            if (!bookings.Any())
            {
                return NotFound("No bookings found for the specified customer.");
            }
            return Ok(bookings);
        }
        /*------------------------------------------------ G U E S T ------------------------------------------------------*/

        [Tags("Guest")]
        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (isAvailable, reason) = await _bookingService
                .IsCampSpotAvailableAsync(request.CampSpotId, request.StartDate, request.EndDate, request.NumberOfPeople);

            if (!isAvailable)
            {
                return BadRequest(reason);
            }

            try 
            {
                var booking = request.ToBooking();
                var createdBooking = await _bookingService.CreateBookingAsync(booking);
                var response = createdBooking.ToBookingDetailsResponse();
                return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Tags("Guest")]
        [HttpPatch("Addons/{id}")]
        public async Task<IActionResult> UpdateAddons(int id, [FromBody] UpdateAddonsRequest request)
        {
            var (success, errorMessage) = await _bookingService.UpdateBookingAddOnsAsync(id, request);
            if (!success)
            {
                return BadRequest(errorMessage);
            }

            return Ok($"Booking with ID {id} has been updated with the selected add-ons.");
        }

        [Tags("Guest")]
        [HttpPatch("GuestCancelBooking/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var (success, errorMessage) = await _bookingService.CancelBookingAsync(id);
            if (!success)
            {
                return BadRequest(errorMessage);
            }

            return Ok($"Booking with ID {id} has been cancelled. See u next time.");
        }

        /*----------------------------------------------- BOOKING MISC ----------------------------------------------------*/





        
    }
}
