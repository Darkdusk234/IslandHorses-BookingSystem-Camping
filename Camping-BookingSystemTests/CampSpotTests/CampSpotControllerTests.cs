using Camping_BookingSystem.Controllers;
using Camping_BookingSystem.Services;
using Moq;

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
    public void TestMethod1()
    {
    }
}
