using System;
using System.Threading.Tasks;
using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
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
            Status = BookingStatus.Pending
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


    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
