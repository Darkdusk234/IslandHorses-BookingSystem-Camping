using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Controllers;
using Camping_BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace Camping_BookingSystemTests;

[TestClass]
public class CampSpotControllerTests
{
    private CampSpotController _campSpotController;
    private Mock<ICampSpotService> _campSpotServiceMock;

    [TestInitialize]
    public void Initialize()
    {
        _campSpotServiceMock = new Mock<ICampSpotService>();

        _campSpotController = new CampSpotController(_campSpotServiceMock.Object);
    }

    [TestMethod]
    public async Task GetAllCampSpots_WhenGettingAllCampSpots_OkStatusCodeWithListOfAllCampSpots()
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
            Id = 1,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = false
        };
        var campSpot3 = new CampSpot
        {
            Id = 1,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = false
        };
        var list = new List<CampSpot> { campSpot1, campSpot2, campSpot3 };
        _campSpotServiceMock.Setup(m => m.GetAllCampSpotsAsync()).ReturnsAsync(list);
        OkObjectResult expected = new OkObjectResult(list);

        var actual = await _campSpotController.GetAllCampSpots();

        Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        _campSpotServiceMock.Verify(repo => repo.GetAllCampSpotsAsync(), Times.Once());
    }

    [TestMethod]
    public async Task GetCampSpotById_WhenInputtingAnExistingId_OkStatusCodeWithCampSpot()
    {
        var id = 1;
        var campSpot = new CampSpot
        {
            Id = 1,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = false
        };
        _campSpotServiceMock.Setup(m => m.GetCampSpotByIdAsync(It.IsAny<int>())).ReturnsAsync(campSpot);
        var expected = new OkObjectResult(campSpot);

        var actual = await _campSpotController.GetCampSpotById(id);

        Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        _campSpotServiceMock.Verify(repo => repo.GetCampSpotByIdAsync(It.IsAny<int>()), Times.Once());
    }
    
    [TestMethod]
    public async Task GetCampSpotById_WhenInputtingAnNonExistingId_NotFoundStatusCodeWithErrorMessage()
    {
        var id = 210;
        _campSpotServiceMock.Setup(m => m.GetCampSpotByIdAsync(It.IsAny<int>())).ReturnsAsync((CampSpot)null);
        var expected = new NotFoundObjectResult("Camp spot not found.");

        var actual = await _campSpotController.GetCampSpotById(id);

        Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        _campSpotServiceMock.Verify(repo => repo.GetCampSpotByIdAsync(It.IsAny<int>()), Times.Once());
    }

    [TestMethod]
    public async Task GetCampSpotByCampSiteId_WhenInputtingAnExistingId_OkStatusCodeWithCampSpots()
    {
        var id = 1;
        var campSpot1 = new CampSpot
        {
            Id = 1,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = false
        };
        var campSpot2 = new CampSpot
        {
            Id = 1,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = true
        };
        var campSpot3 = new CampSpot
        {
            Id = 1,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = true
        };
        var list = new List<CampSpot> { campSpot1, campSpot2, campSpot3 };
        _campSpotServiceMock.Setup(m => m.GetCampSpotsByCampSiteIdAsync(It.IsAny<int>())).ReturnsAsync((list, true));
        var expected = new OkObjectResult(list);

        var actual = await _campSpotController.GetCampSpotsByCampSiteId(id);
        
        Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        _campSpotServiceMock.Verify(repo => repo.GetCampSpotsByCampSiteIdAsync(It.IsAny<int>()), Times.Once());
    }

    [TestMethod]
    public async Task GetCampSpotByCampSiteId_WhenInputtingAnNonExistingId_OkStatusCodeWithCampSpots()
    {
        var id = 20143;
        _campSpotServiceMock.Setup(m => m.GetCampSpotsByCampSiteIdAsync(It.IsAny<int>())).ReturnsAsync((null, false));
        var expected = new NotFoundObjectResult("Campsite not found.");

        var actual = await _campSpotController.GetCampSpotsByCampSiteId(id);

        Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        _campSpotServiceMock.Verify(repo => repo.GetCampSpotsByCampSiteIdAsync(It.IsAny<int>()), Times.Once());
    }
}
