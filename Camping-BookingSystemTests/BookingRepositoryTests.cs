using BookingSystem_ClassLibrary.Data;
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

    [TestMethod]
    public void TestMethod1()
    {
    }
}
