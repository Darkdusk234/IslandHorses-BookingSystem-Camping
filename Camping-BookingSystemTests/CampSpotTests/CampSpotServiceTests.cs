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
    private CampingDbContext _context;
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
}
