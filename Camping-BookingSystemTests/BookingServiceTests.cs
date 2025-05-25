using System;
using System.Threading.Tasks;
using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;
using Camping_BookingSystem.Repositories;
using Camping_BookingSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Camping_BookingSystemTests;

[TestClass]
public class BookingServiceTests
{
    private CampingDbContext _context;
    private BookingService _bookingService;
    private BookingRepository _bookingRepository;
    private CampSpotRepository _campSpotRepository;

    private Customer _customer;
    private CampSpot _campSpot;
    private Booking _booking;

    [TestInitialize]
    public async Task Initialize()
    {
        _context = new CampingDbContext(new DbContextOptionsBuilder<CampingDbContext>()
            .UseInMemoryDatabase($"BookingServiceTestDb_{Guid.NewGuid()}")
            .Options);
        _bookingRepository = new BookingRepository(_context);
        _campSpotRepository = new CampSpotRepository(_context);
        _bookingService = new BookingService(_bookingRepository, _campSpotRepository);

        var spotType = new SpotType { Name = "Tent", Price = 300 };
        var campSite = new CampSite { Name = "Freddans Camping" };
        _customer = new Customer { FirstName = "Freddan", LastName = "Jonsson" };
        _campSpot = new CampSpot
        {
            Electricity = true,
            SpotType = spotType,
            CampSite = campSite
        };

        _booking = new Booking
        {
            Customer = _customer,
            CampSpot = _campSpot,
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3,
            Status = BookingStatus.Pending,
            Wifi = false,
            Parking = false
        };

        await _context.SpotTypes.AddAsync(spotType);
        await _context.CampSites.AddAsync(campSite);
        await _context.Customers.AddAsync(_customer);
        await _context.CampSpots.AddAsync(_campSpot);
        await _context.Bookings.AddAsync(_booking);
        await _context.SaveChangesAsync();
    }

    [TestMethod]
    public async Task CancelBookingsAsync_ShouldCancelBooking_WhenValid()
    {
        //Given: A booking that is in a cancellable state (Pending)
        //When: The booking is cancelled
        var result = await _bookingService.CancelBookingAsync(_booking.Id);
        //Then: Expect the booking to be cancelled successfully
        Assert.IsTrue(result.Success);
        Console.WriteLine($"Status of booking with id {_booking.Id} is now {_booking.Status}");
    }
    [TestMethod]
    public async Task CancelBookingsAsync_ShouldReturnError_WhenBookingNotFound()
    {
        //Given: Nothing?
        //When: Trying to cancel a booking with a non-existing ID
        var result = await _bookingService.CancelBookingAsync(999); // Non-existing booking ID
        //Then: Expect an error message indicating the booking was not found
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.ErrorMEssage);
        Assert.AreEqual("Booking not found", result.ErrorMEssage);
    }

    [TestMethod]
    public async Task CancelBookingsAsync_ShouldReturnError_WhenBookingAlreadyCancelled()
    {
        //Given: The booking is already cancelled
        _booking.Status = BookingStatus.Cancelled;
        _context.Bookings.Update(_booking);
        await _context.SaveChangesAsync();
        //When: Trying to cancel the booking
        var result = await _bookingService.CancelBookingAsync(_booking.Id);
        //Then: Expect an error message indicating the booking is already cancelled
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.ErrorMEssage);
        Assert.AreEqual("Booking is already cancelled", result.ErrorMEssage);
    }

    [TestMethod]
    public async Task CancelBookingsAsync_ShouldReturnError_WhenBookingAlreadyCompleted()
    {
        //Given: The booking is already completed
        _booking.Status = BookingStatus.Completed;
        _context.Bookings.Update(_booking);
        await _context.SaveChangesAsync();
        //When: Trying to cancel the booking
        var result = await _bookingService.CancelBookingAsync(_booking.Id);
        //Then: Expect an error message indicating the booking is already completed
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.ErrorMEssage);
        Assert.AreEqual("Booking is already completed", result.ErrorMEssage);
    }

    [TestMethod]
    public async Task GetBookingByIdAsync_ShouldReturnBooking_WhenExists()
    {
        //Given: A booking exists in the database
        //When: The booking is retrieved by ID
        var result = await _bookingService.GetBookingByIdAsync(_booking.Id);
        //Then: Expect the booking to be returned
        Assert.IsNotNull(result);
        Assert.AreEqual(_booking.Id, result.Id);
        Console.WriteLine($"Booking with ID:{_booking.Id} belongs to {_customer.FirstName}");
    }

    [TestMethod]
    public async Task GetBookingByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        //Given: No booking with the specified ID exists
        //When: Trying to retrieve a booking with a non-existing ID
        var result = await _bookingService.GetBookingByIdAsync(999); // Non-existing booking ID
        //Then: Expect null to be returned
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetBookingsByCustomerIdAsync_ShouldReturnBookings_WhenExists()
    {
        //Given: A customer has bookings in the database
        //When: The bookings are retrieved by customer ID
        var results = await _bookingService.GetBookingsByCustomerIdAsync(_customer.Id);
        //Then: Expect the bookings to be returned
        Assert.IsNotNull(results);
        Assert.IsTrue(results.Count() > 0);
        Console.WriteLine($"Customer {_customer.FirstName} has {results.Count()} bookings.");
    }

    [TestMethod]
    public async Task DeleteBookingAsync_ShouldRemoveBooking_WhenExists()
    {
        //Given: A booking exists in the database
        //When: The booking is deleted
        await _bookingService.DeleteBookingAsync(_booking.Id);
        //Then: Expect the booking to be removed successfully
        var deletedBooking = await _bookingRepository.GetByIdAsync(_booking.Id);

        Assert.IsNull(deletedBooking);
        Console.WriteLine($"Booking with ID:{_booking.Id} has been removed from the database.");
    }

    [TestMethod]
    public async Task DeleteBookingAsync_ShouldDoNothing_WhenBookingDoesNotExist()
    {
        //Given: No booking with the specified ID exists
        //When: Trying to delete a booking with a non-existing ID
        await _bookingService.DeleteBookingAsync(999); // Non-existing booking ID
        //Then: Expect no exception and no change in the database
        var result = await _bookingRepository.GetByIdAsync(999);
        Assert.IsNull(result);
        Console.WriteLine("No changes made to the database for non-existing booking.");
    }

    [TestMethod]
    public async Task IsCampSpotAvailableAsync_ShouldReturnFalse_WhenStartDateIsInThePast()
    {   
        //Given: A camp spot with a booking in the past
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now.AddDays(1);
        //When: Checking availability for the past date
        var (isAvailable, reason) = await _bookingService.IsCampSpotAvailableAsync(_campSpot.Id, startDate, endDate, 2);
        //Then: Expect availability to be false and a reason provided
        Assert.IsFalse(isAvailable);
        Assert.IsNotNull(reason);
        Console.WriteLine($"Availability check for past date returned: {reason}");
    }

    [TestMethod]
    public async Task IsCampSpotAvailableAsync_ShouldReturnFalse_WhenEndDateIsBeforeStartDate()
    {
        //Given: A camp spot with an end date before the start date
        var startDate = DateTime.Now.AddDays(2);
        var endDate = DateTime.Now.AddDays(1);
        //When: Checking availability for the invalid date range
        var (isAvailable, reason) = await _bookingService.IsCampSpotAvailableAsync(_campSpot.Id, startDate, endDate, 2);
        //Then: Expect availability to be false and a reason provided
        Assert.IsFalse(isAvailable);
        Assert.IsNotNull(reason);
        Console.WriteLine($"Availability check for invalid date range returned: {reason}");
    }

    [TestMethod]
    public async Task IsCampSpotAvailableAsync_ShouldReturnFalse_WhenCampSpotNotFound()
    {
        //Given: A non-existing camp spot ID
        var nonExistingCampSpotId = 999;
        var startDate = DateTime.Now.AddDays(1);
        var endDate = DateTime.Now.AddDays(2);
        //When: Checking availability for the non-existing camp spot
        var (isAvailable, reason) = await _bookingService.IsCampSpotAvailableAsync(nonExistingCampSpotId, startDate, endDate, 2);
        //Then: Expect availability to be false and a reason provided
        Assert.IsFalse(isAvailable);
        Assert.IsNotNull(reason);
        Console.WriteLine($"Availability check for non-existing camp spot returned: {reason}");
    }

    [TestMethod]
    public async Task IsCampSpotAvailableAsync_ShouldReturnFalse_WhenOverlappingBookingExists()
    {
        //Given: A camp spot with an existing booking
        var existingBooking = new Booking
        {
            CampSpot = _campSpot,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(3),
            NumberOfPeople = 2
        };
        await _bookingRepository.AddAsync(existingBooking);
        await _bookingRepository.SaveAsync();
        //When: Checking availability for overlapping dates
        var (isAvailable, reason) = await _bookingService.IsCampSpotAvailableAsync(_campSpot.Id, existingBooking.StartDate, existingBooking.EndDate, 2);
        //Then: Expect availability to be false and a reason provided
        Assert.IsFalse(isAvailable);
        Assert.IsNotNull(reason);
        Console.WriteLine($"Availability check for overlapping booking returned: {reason}");
    }

    [TestMethod]
    public async Task IsCampSpotAvailableAsync_ShouldReturnTrue_WhenCampSpotIsAvailable()
    {
        //Given: A camp spot with no bookings in the specified date range
        var startDate = DateTime.Now.AddDays(4);
        var endDate = DateTime.Now.AddDays(5);
        //When: Checking availability for the future date range
        var (isAvailable, reason) = await _bookingService.IsCampSpotAvailableAsync(_campSpot.Id, startDate, endDate, 2);
        //Then: Expect availability to be true and no reason provided
        Assert.IsTrue(isAvailable);
        Assert.IsNull(reason);
        Console.WriteLine($"Availability check for future date range returned: {isAvailable}");
    }

    [TestMethod]
    public async Task UpdateBookingAddOnsAsync_ShouldUpdate_WhenValid()
    {
        //Given: A booking with no add-ons (Wifi and Parking set to false)
        var request = new UpdateAddonsRequest
        {
            Wifi = true,
            Parking = true
        };
        //When: The add-ons are updated
        var result = await _bookingService.UpdateBookingAddOnsAsync(_booking.Id, request);
        //Then: Expect the booking to be updated successfully
        Assert.IsTrue(result.Success);
        var updatedBooking = await _bookingRepository.GetByIdAsync(_booking.Id);
        Assert.IsTrue(updatedBooking.Wifi);
        Assert.IsTrue(updatedBooking.Parking);
        Console.WriteLine($"Wifi: {updatedBooking.Wifi} Parking: {updatedBooking.Parking}");
    }

    [TestMethod]
    public async Task UpdateBookingAddOnsAsync_ShouldReturnError_WhenBookingNotFound()
    {
        //Given: A non-existing booking ID
        var request = new UpdateAddonsRequest
        {
            Wifi = true,
            Parking = true
        };
        //When: Trying to update add-ons for a non-existing booking
        var result = await _bookingService.UpdateBookingAddOnsAsync(999, request); // Non-existing booking ID
        //Then: Expect an error message indicating the booking was not found
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.ErrorMessage);
        Assert.AreEqual("Booking not found", result.ErrorMessage);
    }

    [TestMethod]
    public async Task UpdateBookingAddOnsAsync_ShouldFail_WhenBookingIsCompleted()
    {
        //Given: A booking that is already completed
        _booking.Status = BookingStatus.Completed;
        _context.Bookings.Update(_booking);
        await _context.SaveChangesAsync();
        
        //When: Trying to update add-ons for a completed booking
        var result = await _bookingService.UpdateBookingAddOnsAsync(_booking.Id, new UpdateAddonsRequest());
        //Then: Expect an error message indicating the booking is already completed or cancelled
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.ErrorMessage);
        Assert.AreEqual("Booking is either completed or cancelled.", result.ErrorMessage);
    }

    [TestMethod]
    public async Task UpdateBookingAsync_ShouldUpdate_WhenValid()
    {
        //Given: A booking with valid details
        var updatedBooking = new UpdateBookingRequest
        {
            CustomerId = _customer.Id,
            CampSpotId = _campSpot.Id,
            StartDate = DateTime.Now.AddDays(3),
            EndDate = DateTime.Now.AddDays(5),
            NumberOfPeople = 4,
            Wifi = true,
            Parking = true,
            Status = BookingStatus.Confirmed
        };
        //When: The booking is updated
        var update = await _bookingService.UpdateBookingAsyn(_booking.Id, updatedBooking);
        Assert.IsTrue(update.Success);

        var result = await _bookingRepository.GetByIdAsync(_booking.Id);
        await _bookingRepository.SaveAsync();
        //Then: Expect the booking to be updated successfully
        Assert.IsNotNull(result);
        Assert.AreEqual(updatedBooking.StartDate, result.StartDate);
        Assert.AreEqual(updatedBooking.EndDate, result.EndDate);
        Assert.AreEqual(updatedBooking.NumberOfPeople, result.NumberOfPeople);
        Console.WriteLine($"Updated booking with ID:{result.Id} has new start date: {result.StartDate}\n" +
            $"Wifi: {updatedBooking.Wifi} / Parking: {updatedBooking.Parking}\n" +
            $"Status: {updatedBooking.Status} / Number of people: {updatedBooking.NumberOfPeople}");
    }

    [TestMethod]
    public async Task UpdateBookingAsync_ShouldFail_WhenBookingNotFound()
    {
        //Given: A non-existing booking ID
        var updatedBooking = new UpdateBookingRequest
        {
            CustomerId = _customer.Id,
            CampSpotId = _campSpot.Id,
            StartDate = DateTime.Now.AddDays(3),
            EndDate = DateTime.Now.AddDays(5),
            NumberOfPeople = 4,
            Wifi = true,
            Parking = true,
            Status = BookingStatus.Confirmed
        };
        //When: Trying to update a booking with a non-existing ID
        var result = await _bookingService.UpdateBookingAsyn(999, updatedBooking); // Non-existing booking ID
        //Then: Expect an error message indicating the booking was not found
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.ErrorMessage);
        Assert.AreEqual("Booking not found", result.ErrorMessage);
    }
        [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
