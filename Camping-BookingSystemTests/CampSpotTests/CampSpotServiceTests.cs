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
    public void AddCampSpotAsync_InputtingValidCampSpot_CampSpotThatWasAdded()
    {
        var campSpot = new CampSpot
        {
            Id = 1,
            CampSiteId = 1,
            TypeId = 1,
            Electricity = true
        };

        _campSpotRepoMock.Setup(m => m.Create(It.IsAny<CampSpot>())).Returns(Task.CompletedTask);

        var result = _campSpotService.AddCampSpotAsync(campSpot).Result;

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
}
