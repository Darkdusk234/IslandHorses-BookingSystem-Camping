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
}
