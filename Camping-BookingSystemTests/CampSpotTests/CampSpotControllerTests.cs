using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CampSpotDTOs;
using Camping_BookingSystem.Controllers;
using Camping_BookingSystem.Mapping;
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

    [TestMethod]
    public async Task CreateCampSpot_WhenValidCampSpotDataIsInputted_CreatedAtActionStatusCodeWithCampSpotCreated()
    {
        var campSpotToCreate = new CreateCampSpotRequest
        {
            CampSiteId = 1,
            TypeId = 2,
            Electricity = false
        };
        var campSpot = campSpotToCreate.ToCampSpot();
        _campSpotServiceMock.Setup(m => m.AddCampSpotAsync(It.IsAny<CampSpot>())).ReturnsAsync(campSpot);
        var expected = new CreatedAtActionResult("GetCampSpotById", null, new { id = campSpot.Id }, campSpot);

        var actual = await _campSpotController.CreateCampSpot(campSpotToCreate);

        Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        _campSpotServiceMock.Verify(repo => repo.AddCampSpotAsync(It.IsAny<CampSpot>()), Times.Once());
    }

    [TestMethod]
    public async Task CreateCampSpot_WhenMethodEncountersErrorTryingToAddData_BadRequestWithErrorMessage()
    {
        var campSpotToCreate = new CreateCampSpotRequest
        {
            CampSiteId = 1,
            TypeId = 2,
            Electricity = false
        };
        var campSpot = campSpotToCreate.ToCampSpot();
        var exception = new ArgumentException("Error happened.");
        _campSpotServiceMock.Setup(m => m.AddCampSpotAsync(It.IsAny<CampSpot>())).Throws(exception);
        var expected = new BadRequestObjectResult("Error happened.");

        var actual = await _campSpotController.CreateCampSpot(campSpotToCreate);

        Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        _campSpotServiceMock.Verify(repo => repo.AddCampSpotAsync(It.IsAny<CampSpot>()), Times.Once());
    }

    [TestMethod]
    public async Task UpdateCampSpot_WhenExistingIdAndValidDataIsInputted_NoContentStatusCode()
    {
        var id = 1;
        var campSpotToUpdate = new CreateCampSpotRequest
        {
            CampSiteId = 1,
            TypeId = 2,
            Electricity = false
        };
        _campSpotServiceMock.Setup(m => m.UpdateCampSpotAsync(It.IsAny<int>(), It.IsAny<CreateCampSpotRequest>())).ReturnsAsync((true, null));
        var expected = new NoContentResult();
        
        var actual = await _campSpotController.UpdateCampSpot(id, campSpotToUpdate);

        Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        _campSpotServiceMock.Verify(repo => repo.UpdateCampSpotAsync(It.IsAny<int>(), It.IsAny<CreateCampSpotRequest>()), Times.Once());
    }

    [TestMethod]
    public async Task UpdateCampSpot_WhenNonExistingIdAndValidDataIsInputted_NoTFoundStatusCodeWithErrorMessage()
    {
        var id = 2415;
        var campSpotToUpdate = new CreateCampSpotRequest
        {
            CampSiteId = 1,
            TypeId = 2,
            Electricity = false
        };
        _campSpotServiceMock.Setup(m => m.UpdateCampSpotAsync(It.IsAny<int>(), It.IsAny<CreateCampSpotRequest>())).ReturnsAsync((false, "Camp spot not found."));
        var expected = new NotFoundObjectResult("Camp spot not found.");

        var actual = await _campSpotController.UpdateCampSpot(id, campSpotToUpdate);

        Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        _campSpotServiceMock.Verify(repo => repo.UpdateCampSpotAsync(It.IsAny<int>(), It.IsAny<CreateCampSpotRequest>()), Times.Once());
    }
}
