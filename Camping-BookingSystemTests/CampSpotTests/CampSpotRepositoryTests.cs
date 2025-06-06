using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Camping_BookingSystemTests.CampSpotTests;

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
            Electricity = true
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
            Electricity = true
        };

        context.Add(campSpot1);
        await context.SaveChangesAsync();

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
            Electricity = true
        };
        var campSpot2 = new CampSpot
        {
            Id = 2,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false
        };
        context.Add(campSpot1);
        context.Add(campSpot2);
        await context.SaveChangesAsync();

        var actual = await repository.GetAll();

        Assert.IsTrue(actual.Count() > 0);
    }

    [TestMethod]
    public async Task GetByCampSiteId_SearchForCampSpotsWithExistingCampSiteId_ListOfCampSpotsConnectedToCampSite()
    {
        var context = GetInMemoryDbContext();
        var repository = new CampSpotRepository(context);
        var campSpot1 = new CampSpot
        {
            Id = 5,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = true
        };
        var campSpot2 = new CampSpot
        {
            Id = 2,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false
        };
        var campSpot3 = new CampSpot
        {
            Id = 7,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false
        };
        var campSpot4= new CampSpot
        {
            Id = 1,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false
        };

        context.Add(campSpot1);
        context.Add(campSpot2);
        context.Add(campSpot3);
        context.Add(campSpot4);
        await context.SaveChangesAsync();

        var actual = await repository.GetCampSpotsByCampSiteId(3);

        Assert.IsTrue(actual.Count() > 0);
        Assert.AreEqual(3, actual.First().CampSiteId);
    }

    [TestMethod]
    public async Task GetByCampSiteId_SearchForCampSpotsWithNonExistingCampSiteId_EmptyList()
    {
        var context = GetInMemoryDbContext();
        var repository = new CampSpotRepository(context);
        var campSpot1 = new CampSpot
        {
            Id = 5,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = true
        };
        var campSpot2 = new CampSpot
        {
            Id = 2,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false
        };
        var campSpot3 = new CampSpot
        {
            Id = 7,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false
        };
        var campSpot4 = new CampSpot
        {
            Id = 1,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false
        };

        context.Add(campSpot1);
        context.Add(campSpot2);
        context.Add(campSpot3);
        context.Add(campSpot4);
        await context.SaveChangesAsync();

        var actual = await repository.GetCampSpotsByCampSiteId(11);

        Assert.IsTrue(actual.Count() == 0);
    }

    [TestMethod]
    public async Task GetCampSpotById_SearchForCampSpotWithExistingId_CampSpotWithInputtedId()
    {
        var context = GetInMemoryDbContext();
        var repository = new CampSpotRepository(context);
        var campSpot1 = new CampSpot
        {
            Id = 5,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = true
        };
        var campSpot2 = new CampSpot
        {
            Id = 2,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false
        };
        context.Add(campSpot1);
        context.Add(campSpot2);
        await context.SaveChangesAsync();

        var actual = await repository.GetCampSpotById(2);

        Assert.AreEqual(JsonConvert.SerializeObject(campSpot2), JsonConvert.SerializeObject(actual));
    }

    [TestMethod]
    public async Task GetCampSpotById_SearchForCampSpotWithNonExistingId_Null()
    {
        var context = GetInMemoryDbContext();
        var repository = new CampSpotRepository(context);
        var campSpot1 = new CampSpot
        {
            Id = 5,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = true
        };
        var campSpot2 = new CampSpot
        {
            Id = 2,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false
        };
        context.Add(campSpot1);
        context.Add(campSpot2);
        await context.SaveChangesAsync();

        var actual = await repository.GetCampSpotById(6);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public async Task Update_InputtingExistingCampSpot_CampSpotInDatabaseUpdated()
    {
        var context = GetInMemoryDbContext();
        var repository = new CampSpotRepository(context);
        var campSpot1 = new CampSpot
        {
            Id = 5,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = true
        };
        var campSpot2 = new CampSpot
        {
            Id = 2,
            CampSiteId = 3,
            TypeId = 5,
            Electricity = false
        };
        context.Add(campSpot1);
        context.Add(campSpot2);
        await context.SaveChangesAsync();

        var campSpotToUpdate = await repository.GetCampSpotById(2);
        var campSpotBeforeUpdate = JsonConvert.SerializeObject(campSpotToUpdate);

        campSpotToUpdate.Electricity = true;
        await repository.Update(campSpotToUpdate);
        context.ChangeTracker.Clear();

        var actual = await repository.GetCampSpotById(2);

        Assert.AreNotEqual(campSpotBeforeUpdate, JsonConvert.SerializeObject(actual));
        Assert.AreEqual(JsonConvert.SerializeObject(campSpotToUpdate), JsonConvert.SerializeObject(actual));
    }
}
