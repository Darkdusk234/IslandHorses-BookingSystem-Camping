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

    [TestMethod]
    public async Task Delete_DeleteInputtedCampSpotFromDatabase_CampSpotDeletedFromDatabase()
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

        await repository.Delete(campSpot1);

        var actual = await repository.GetCampSpotById(5);
        Assert.IsNull(actual);
    }

    [TestMethod]
    public async Task GetAll_GetAllCampSpotsInDatabase_ListOfAllCampSpotsInDatabase()
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
        var campSpot2 = new CampSpot
        {
            Id = 2,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false,
            MaxPersonLimit = 9
        };
        await repository.Create(campSpot1);
        await repository.Create(campSpot2);

        var actual = await repository.GetAll();

        Assert.IsTrue(actual.Count() > 0);
    }

    [TestMethod]
    public async Task GetByCampSiteId_GetAllCampSpotsInDatabaseConnectedToInputtedCampSiteId_ListOfCampSpotsConnectedToCampSite()
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
        var campSpot2 = new CampSpot
        {
            Id = 2,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false,
            MaxPersonLimit = 9
        };
        var campSpot3 = new CampSpot
        {
            Id = 7,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false,
            MaxPersonLimit = 9
        };
        var campSpot4= new CampSpot
        {
            Id = 1,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false,
            MaxPersonLimit = 9
        };
        await repository.Create(campSpot1);
        await repository.Create(campSpot2);
        await repository.Create(campSpot3);
        await repository.Create(campSpot4);

        var actual = await repository.GetByCampSiteId(3);

        Assert.IsTrue(actual.Count() > 0);
        Assert.AreEqual(3, actual.First().CampSiteId);
    }
}
