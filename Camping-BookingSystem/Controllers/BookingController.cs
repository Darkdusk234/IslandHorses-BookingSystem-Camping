﻿using BookingSystem_ClassLibrary.Models;
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
            var bookings = await _bookingService.GetAllBookingsAsync();
            var response = bookings.Select(b => b.ToBookingDetailsResponse());
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
            var response = bookings.Select(b => b.ToBookingDetailsResponse());
            return Ok(response);
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
            return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingRequest request)
        {
            var existingBooking = await _bookingService.GetBookingByIdAsync(id);
            if (existingBooking == null)
            {
                return NotFound();
            }
            
            existingBooking.CampSpotId = request.CampSpotId;
            existingBooking.CustomerId = request.CustomerId;
            existingBooking.StartDate = request.StartDate;
            existingBooking.EndDate = request.EndDate;
            existingBooking.NumberOfPeople = request.NumberOfPeople;
            // existingBooking.Status = request.Status; // Assuming status is part of the request

            await _bookingService.UpdateBookingAsync(existingBooking);
            return NoContent();
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
