using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Camping_BookingSystemTests;

[TestClass]
public class BookingRepositoryTests
{
    private CampingDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CampingDbContext>()
            .UseInMemoryDatabase($"BookingTestDb_{Guid.NewGuid()}")
            .Options;
        return new CampingDbContext(options);
    }
    /*------------------------------------------------------------------*/
    [TestMethod]
    public async Task AddAsync_ShouldAddBookingToDataBase()
    {
        //Given:  A new in-memory database and a booking repository and a booking object to be added

        using var context = GetInMemoryDbContext();
        var repository = new BookingRepository(context);


        var booking = new Booking
        {
            CustomerId = 1,
            CampSpotId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3
        };

        //When: Added & saved to the database
        await repository.AddAsync(booking);
        await repository.SaveAsync();

        //Then: Expect the booking to not be null and to be added in the database
        var result = await context.Bookings.FirstOrDefaultAsync();
        Assert.IsNotNull(result);
        Assert.AreEqual(booking.CustomerId, result.CustomerId);
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnAllBookings()
    {
        //Given:  A new in-memory database and a booking repository and a booking object to be added
        using var context = GetInMemoryDbContext();
        var repository = new BookingRepository(context);
        var booking1 = new Booking
        {
            CustomerId = 1,
            CampSpotId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3
        };
        var booking2 = new Booking
        {
            CustomerId = 2,
            CampSpotId = 2,
            StartDate = DateTime.Now.AddDays(3),
            EndDate = DateTime.Now.AddDays(5),
            NumberOfPeople = 4
        };
        await repository.AddAsync(booking1);
        await repository.AddAsync(booking2);
        await repository.SaveAsync();
        //When: All bookings are retrieved from the database
        var result = await repository.GetAllAsync();
        //Then: Expect the result to contain both bookings
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetBookingById_ShouldReturnBooking()
    {
        //Given:  A new in-memory database and a booking repository and a booking object to be added
        using var context = GetInMemoryDbContext();
        var repository = new BookingRepository(context);
        var booking = new Booking
        {
            CustomerId = 1,
            CampSpotId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3
        };
        context.Bookings.Add(booking);
        context.SaveChanges();
        //When: The booking is retrieved by ID
        var result = repository.GetByIdAsync(booking.Id).Result;
        //Then: Expect the result to be the same as the added booking
        Assert.IsNotNull(result);
        Assert.AreEqual(booking.CustomerId, result.CustomerId);
    }
    [TestMethod]
    public async Task UpdateBooking_ShouldModifyExistingBooking()
    {
        //Given:  A new in-memory database and a booking repository and a booking object to be added
        using var context = GetInMemoryDbContext();
        var repository = new BookingRepository(context);
        var booking = new Booking
        {
            CustomerId = 1,
            CampSpotId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3
        };
        context.Bookings.Add(booking);
        context.SaveChanges();
        //When: The booking is updated by how many people
        booking.NumberOfPeople = 5;
        repository.Update(booking);
        await repository.SaveAsync();
        //Then: Expect the updated booking to have the new number of people
        var result = await repository.GetByIdAsync(booking.Id);
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.NumberOfPeople);
    }

    [TestMethod]
    public async Task DeleteBooking_ShouldRemoveBookingFromDataBase()
    {
        //Given:  A new in-memory database and a booking repository and a booking object to be added    
        using var context = GetInMemoryDbContext();
        var repository = new BookingRepository(context);

        var booking = new Booking
        {
            CustomerId = 1,
            CampSpotId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 3
        };
        await repository.AddAsync(booking);
        await repository.SaveAsync();

        //When: The booking is deleted from the database
        repository.Delete(booking);
        await repository.SaveAsync();

        //Then: Expect the booking to be removed from the database
        var deleted = await repository.GetByIdAsync(booking.Id);
        Assert.IsNull(deleted);
    }

    [TestMethod]
    public async Task GetBookingsByCustomerIdAsync_ShouldReturnCorrectBookings()
    {
        //Given:  A new in-memory database and a booking repository and multiple new booking objects to be added
        var context = GetInMemoryDbContext();
        var repository = new BookingRepository(context);

        await repository.AddAsync(new Booking { CustomerId = 1, CampSpotId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2), NumberOfPeople = 3 });
        await repository.AddAsync(new Booking { CustomerId = 1, CampSpotId = 3, StartDate = DateTime.Now.AddDays(6), EndDate = DateTime.Now.AddDays(8), NumberOfPeople = 2 });
        await repository.AddAsync(new Booking { CustomerId = 2, CampSpotId = 2, StartDate = DateTime.Now.AddDays(3), EndDate = DateTime.Now.AddDays(5), NumberOfPeople = 4 });
        await repository.SaveAsync();

        //When: The bookings are retrieved by customer ID
        var results = await repository.GetBookingsByCustomerIdAsync(1);

        //Then: Expect the result to contain two bookings for customer with ID 1
        Assert.AreEqual(2, results.Count());
        Assert.IsTrue(results.All(b => b.CustomerId == 1));
    }
}
