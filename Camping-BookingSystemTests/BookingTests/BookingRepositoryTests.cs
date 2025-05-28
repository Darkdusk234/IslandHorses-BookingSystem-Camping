using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Camping_BookingSystemTests.BookingTests;

[TestClass]
public class BookingRepositoryTests
{
    private CampingDbContext _context;
    private BookingRepository _repository;
    private Customer _customer;
    private CampSpot _campSpot;
    private SqliteConnection _connection;

    [TestInitialize]
    public async Task Initialize() 
    {
        DbContextOptions<CampingDbContext> options;
        
            options = new DbContextOptionsBuilder<CampingDbContext>()
                .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
                .Options;
        

        _context = new CampingDbContext(options);
        

        _repository = new BookingRepository(_context);

        var spotType = new SpotType { Name = "Tent", Price = 300 };
        var campSite = new CampSite { Name = "Freddans Camping" };

        _customer = new Customer { FirstName = "Freddan", LastName = "Jonsson" };
        _campSpot = new CampSpot
        {
            Electricity = true,
            SpotType = spotType,
            CampSite = campSite
        };

        await _context.SpotTypes.AddAsync(spotType);
        await _context.CampSites.AddAsync(campSite);
        await _context.Customers.AddAsync(_customer);
        await _context.CampSpots.AddAsync(_campSpot);
        await _context.SaveChangesAsync();

    }

    /*------------------------------------------------------------------------------------------------------*/
    [TestMethod]
    public async Task AddAsync_ShouldAddBookingToDataBase()
    {
        //Given:  A new in-memory database and a booking repository and a booking object to be added
        var booking = new Booking
        {
            CustomerId = 1,
            CampSpotId = _campSpot.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3
        };

        //When: Added & saved to the database
        await _repository.AddAsync(booking);
        await _repository.SaveAsync();

        //Then: Expect the booking to not be null and to be added in the database
        var result = await _context.Bookings.FirstOrDefaultAsync();
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.CustomerId);
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnAllBookings()
    {
        //Given:  A new in-memory database and a booking repository and 2 booking objects to be added

        var booking1 = new Booking
        {
            CustomerId = _customer.Id,
            CampSpotId = _campSpot.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3
        };
        var booking2 = new Booking
        {
            CustomerId = _customer.Id,
            CampSpotId = _campSpot.Id,
            StartDate = DateTime.Now.AddDays(3),
            EndDate = DateTime.Now.AddDays(5),
            NumberOfPeople = 4
        };
        await _repository.AddAsync(booking1);
        await _repository.AddAsync(booking2);
        await _repository.SaveAsync();
        //When: All bookings are retrieved from the database
        var result = (await _repository.GetAllAsync()).ToList();
        //Then: Expect the result to contain both bookings
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetBookingById_ShouldReturnBooking()
    {
        //Given:  A new in-memory database and a booking repository and a booking object to be added
        var booking = new Booking
        {
            CustomerId = 1,
            CampSpotId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3
        };
        _context.Bookings.Add(booking);
        _context.SaveChanges();
        //When: The booking is retrieved by ID
        var result = _repository.GetByIdAsync(booking.Id).Result;
        //Then: Expect the result to be the same as the added booking
        Assert.IsNotNull(result);
        Assert.AreEqual(booking.CustomerId, result.CustomerId);
    }
    [TestMethod]
    public async Task UpdateBooking_ShouldModifyExistingBooking()
    {
        //Given:  A new in-memory database and a booking repository and a booking object to be added
        
        var booking = new Booking
        {
            CustomerId = 1,
            CampSpotId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3
        };
        _context.Bookings.Add(booking);
        _context.SaveChanges();
        //When: The booking is updated by how many people
        booking.NumberOfPeople = 5;
        _repository.Update(booking);
        await _repository.SaveAsync();
        _context.ChangeTracker.Clear(); // Clear the change tracker to ensure we get the latest state from the database
        //Then: Expect the updated booking to have the new number of people
        var result = await _repository.GetByIdAsync(booking.Id);
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.NumberOfPeople);
    }

    [TestMethod]
    public async Task DeleteBooking_ShouldRemoveBookingFromDataBase()
    {
        //Given:  A new in-memory database and a booking repository and a booking object to be added    

        var booking = new Booking
        {
            CustomerId = _customer.Id,
            CampSpotId = _campSpot.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3
        };
        await _repository.AddAsync(booking);
        await _repository.SaveAsync();

        //When: The booking is deleted from the database
        _repository.Delete(booking);
        await _repository.SaveAsync();

        //Then: Expect the booking to be removed from the database
        var deleted = await _repository.GetByIdAsync(booking.Id);
        Assert.IsNull(deleted);
    }
    
    [TestMethod]
    public async Task GetBookingsByCustomerIdAsync_ShouldReturnCorrectBookings()
    {
        //Given: multiple new booking objects to be added

        await _repository.AddAsync(new Booking { CustomerId = 1, CampSpotId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2), NumberOfPeople = 3 });
        await _repository.AddAsync(new Booking { CustomerId = 1, CampSpotId = 1, StartDate = DateTime.Now.AddDays(6), EndDate = DateTime.Now.AddDays(8), NumberOfPeople = 2 });
        await _repository.AddAsync(new Booking { CustomerId = 2, CampSpotId = 2, StartDate = DateTime.Now.AddDays(3), EndDate = DateTime.Now.AddDays(5), NumberOfPeople = 4 });
        await _repository.SaveAsync();

        //When: The bookings are retrieved by customer ID
        var results = await _repository.GetBookingDetailsByCustomerIdAsync(1);

        //Then: Expect the result to contain two bookings for customer with ID 1
        Assert.AreEqual(2, results.Count());
        Assert.IsTrue(results.All(b => b.CustomerId == 1));
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnNull_WhenBookingDoesNotExist()
    {
        //Given:  Nothing.

        //When: The booking is retrieved by a non-existing ID
        var result = await _repository.GetByIdAsync(999);
        //Then: Expect the result to be null
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetBookingByCustomerIdAsync_ShouldReturnEmpty_WhenNoBookingsExist()
    {

        //Given:  A new in-memory database and a booking repository with no bookings


        //When: The bookings are retrieved by a customer ID that has no bookings
        var results = await _repository.GetBookingDetailsByCustomerIdAsync(_customer.Id);
        //Then: Expect the result to be empty
        Assert.IsNotNull(results);
        Assert.AreEqual(0, results.Count());
    }

    [TestMethod]
    public async Task Update_ShouldThrow_WhenBookingDoesNotExist()
    {
        //Given:  A booking to update that is not in the database
        var booking = new Booking
        {
            Id = 999,
            CustomerId = _customer.Id,
            CampSpotId = _campSpot.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3
        };
        //When: Update is called and SaveAsync is attempted

        //Then: Expect a DbUpdateConcurrencyException to be thrown
        await Assert.ThrowsExceptionAsync<DbUpdateConcurrencyException>(async () =>
        {
            _repository.Update(booking);
            await _repository.SaveAsync();
        });
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
