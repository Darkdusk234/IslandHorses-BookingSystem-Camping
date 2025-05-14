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
}
