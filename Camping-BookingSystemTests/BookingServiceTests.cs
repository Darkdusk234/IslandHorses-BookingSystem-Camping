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
        var result = await _bookingService.CancelBookingAsync(_booking.Id);

        Assert.IsTrue(result.Success);
        Assert.IsNull(result.ErrorMEssage);
        Console.WriteLine($"Status of booking with id {_booking.Id} is now {_booking.Status}");
    }
    [TestMethod]
    public async Task CancelBookingsAsync_ShouldReturnError_WhenBookingNotFound()
    {
        var result = await _bookingService.CancelBookingAsync(999); // Non-existing booking ID
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.ErrorMEssage);
        Assert.AreEqual("Booking not found", result.ErrorMEssage);
    }

    [TestMethod]
    public async Task CancelBookingsAsync_ShouldReturnError_WhenBookingAlreadyCancelled()
    {
        _booking.Status = BookingStatus.Cancelled;
        _context.Bookings.Update(_booking);
        await _context.SaveChangesAsync();
        var result = await _bookingService.CancelBookingAsync(_booking.Id);
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.ErrorMEssage);
        Assert.AreEqual("Booking is already cancelled", result.ErrorMEssage);
    }

    [TestMethod]

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
