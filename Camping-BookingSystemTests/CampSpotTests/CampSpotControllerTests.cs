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
    public async Task GetAllCampSpots_WhenGettingAllCampSpots_ListOfAllCampSpotsAsync()
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
}
