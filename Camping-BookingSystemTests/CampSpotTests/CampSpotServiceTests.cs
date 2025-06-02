using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Repositories;
using Camping_BookingSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
namespace Camping_BookingSystemTests;

[TestClass]
public class CampSpotServiceTests
{
    private CampSpotService _campSpotService;
    private Mock<ICampSpotRepository> _campSpotRepoMock;
    private Mock<ICampSiteRepository> _campSiteRepoMock;

    [TestInitialize]
    public void Initialize()
    {
        _campSpotRepoMock = new Mock<ICampSpotRepository>();
        _campSiteRepoMock = new Mock<ICampSiteRepository>();

        _campSpotService = new CampSpotService(_campSpotRepoMock.Object, _campSiteRepoMock.Object);
    }

    [TestMethod]
    public async Task AddCampSpotAsync_InputtingValidCampSpot_CampSpotThatWasAddedAsync()
    {
        var campSpot = new CampSpot
        {
            Id = 1,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = true
        };

        _campSpotRepoMock.Setup(m => m.Create(It.IsAny<CampSpot>())).Returns(Task.CompletedTask);

        var result = await _campSpotService.AddCampSpotAsync(campSpot);

        Assert.AreEqual(JsonConvert.SerializeObject(campSpot), JsonConvert.SerializeObject(result));
        _campSpotRepoMock.Verify(repo => repo.Create(It.IsAny<CampSpot>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteCampSpotAsync_InputtingExistingCampSpotId_TrueAndNullErrorMessageAsync()
    {
        int id = 1;
        var campSpot = new CampSpot
        {
            Id = id,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = true
        };

        _campSpotRepoMock.Setup(m => m.GetCampSpotById(It.IsAny<int>())).ReturnsAsync(campSpot);
        _campSpotRepoMock.Setup(m => m.Delete(It.IsAny<CampSpot>())).Returns(Task.CompletedTask);
        var (success, message) = await _campSpotService.DeleteCampSpotAsync(id);

        Assert.AreEqual(success, true);
        Assert.IsNull(message);
        _campSpotRepoMock.Verify(repo => repo.GetCampSpotById(It.IsAny<int>()), Times.Once);
        _campSpotRepoMock.Verify(repo => repo.Delete(It.IsAny<CampSpot>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteCampSpotAsync_InputtingNonExistingCampSpotId_FalseAndErrorMessageAsync()
    {
        int id = 20;

        _campSpotRepoMock.Setup(m => m.GetCampSpotById(It.IsAny<int>())).ReturnsAsync((CampSpot)null);
        var (success, message) = await _campSpotService.DeleteCampSpotAsync(id);

        Assert.IsFalse(success);
        Assert.AreEqual("Camp spot not found.", message);
        _campSpotRepoMock.Verify(repo => repo.GetCampSpotById(It.IsAny<int>()), Times.Once);
    }

    [TestMethod]
    public async Task GetAllCampSpotsAsync_GettingAllCampSpots_IEnumerableOfCampSpots()
    {
        var campSpot1 = new CampSpot
        {
            Id = 1,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = false
        };
        var campSpot2 = new CampSpot
        {
            Id = 2,
            CampSiteId = 4,
            TypeId = 7,
            Electricity = true
        };
        var campSpot3 = new CampSpot
        {
            Id = 3,
            CampSiteId = 4,
            TypeId = 6,
            Electricity = false
        };
        var list = new List<CampSpot> { campSpot1, campSpot2, campSpot3 };
        _campSpotRepoMock.Setup(m => m.GetAll()).ReturnsAsync(list);

        var result = await _campSpotService.GetAllCampSpotsAsync();

        Assert.AreEqual(JsonConvert.SerializeObject(list), JsonConvert.SerializeObject(result));
        _campSpotRepoMock.Verify(repo => repo.GetAll(), Times.Once);
    }
}
