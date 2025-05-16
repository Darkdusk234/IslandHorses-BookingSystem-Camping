using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Camping_BookingSystemTests;

[TestClass]
public class CampSpotRepositoryTests
{
    private CampingDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CampingDbContext>()
            .UseInMemoryDatabase($"BookingTestDb_{Guid.NewGuid()}")
            .Options;
        return new CampingDbContext(options);
    }

    [TestMethod]
    public async Task Create_AddInputtedCampSpotToDatabase_CampSpotAddedToDatabase()
    {
        var context = GetInMemoryDbContext();
        var repository = new CampSpotRepository(context);
        var campSpot1 = new CampSpot
        {
            Id = 5,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = true,
            MaxPersonLimit = 10
        };

        await repository.Create(campSpot1);

        var actual = await repository.GetCampSpotById(5);
        Assert.AreEqual(JsonConvert.SerializeObject(campSpot1), JsonConvert.SerializeObject(actual));
    }
}
