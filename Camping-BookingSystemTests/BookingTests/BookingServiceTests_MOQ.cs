using Camping_BookingSystem.Repositories;
using Camping_BookingSystem.Services;
using Moq;

namespace Camping_BookingSystemTests;

[TestClass]
public class BookingServiceTests_MOQ
{
    private Mock<IBookingRepository> _bookingRepoMock;
    private Mock<ICampSpotRepository> _campSpotRepoMock;
    private Mock<ICustomerRepository> _customerRepoMock;
    private BookingService _bookingService;

    [TestInitialize]
    public void Setup()
    {
        _bookingRepoMock = new Mock<IBookingRepository>();
        _campSpotRepoMock = new Mock<ICampSpotRepository>();
        _customerRepoMock = new Mock<ICustomerRepository>();

        _bookingService = new BookingService(
            _bookingRepoMock.Object,
            _campSpotRepoMock.Object,
            _customerRepoMock.Object
        );
    }



    [TestMethod]
    public void CancelBookingAsync_ShouldCancel_IfStatusIsntCompletedOrCancelled()
    {


        // Arrange
        
        // Act
        // Call methods on bookingService and verify behavior
        // Assert
        // Use assertions to verify expected outcomes
    }
}
