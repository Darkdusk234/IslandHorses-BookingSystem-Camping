using BookingSystem_ClassLibrary.Models;
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
    public void CancelBookingAsync_ShouldCancel_WhenValidStatus()
    {

        // Given: A booking with a status of Pending
        var bookingId = 1;
        var booking = new Booking
        {
            Id = bookingId,
            Status = BookingStatus.Pending
        };
        _bookingRepoMock.Setup(repo => repo.GetByIdAsync(bookingId)).ReturnsAsync(booking);
        _bookingRepoMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);


        // When: The CancelBookingAsync method is called
        var result = _bookingService.CancelBookingAsync(bookingId).Result;

        // Then: Expect the booking status to be changed to Cancelled
        Assert.IsTrue(result.Success);
        Assert.IsNull(result.ErrorMEssage);
        Assert.AreEqual(BookingStatus.Cancelled, booking.Status);
        _bookingRepoMock.Verify(repo => repo.SaveAsync(), Times.Once);
        _bookingRepoMock.Verify(x => x.SaveAsync(), Times.Once);
    }

    [TestMethod]
    public void CancelBookingAsync_ShouldReturnError_WhenBookingNotFound()
    {
        // Given: A booking ID that does not exist
        var bookingId = 999;
        _bookingRepoMock.Setup(repo => repo.GetByIdAsync(bookingId)).ReturnsAsync((Booking?)null);

        // When: The CancelBookingAsync method is called
        var result = _bookingService.CancelBookingAsync(bookingId).Result;

        // Then: Expect the booking to not be cancelled, because it does not exist
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Booking not found", result.ErrorMEssage);
        _bookingRepoMock.Verify(repo => repo.GetByIdAsync(bookingId), Times.Once);
    }

    [TestMethod]
    public void CancelBookingAsync_ShouldReturnError_WhenBookingAlreadyCancelled()
    {
        // Given: A booking that is already cancelled
        var bookingId = 1;
        var booking = new Booking
        {
            Id = bookingId,
            Status = BookingStatus.Cancelled
        };
        _bookingRepoMock.Setup(repo => repo.GetByIdAsync(bookingId)).ReturnsAsync(booking);
        
        // When: The CancelBookingAsync method is called
        var result = _bookingService.CancelBookingAsync(bookingId).Result;
        
        // Then: Expect the booking to not be cancelled, because it is already cancelled
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Booking is already cancelled", result.ErrorMEssage);
        _bookingRepoMock.Verify(repo => repo.GetByIdAsync(bookingId), Times.Once);
    }

    [TestMethod]
    public void CancelBookingAsync_ShouldReturnError_WhenBookingAlreadyCompleted()
    {
        // Given: A booking that is already completed
        var bookingId = 1;
        var booking = new Booking
        {
            Id = bookingId,
            Status = BookingStatus.Completed
        };
        _bookingRepoMock.Setup(repo => repo.GetByIdAsync(bookingId)).ReturnsAsync(booking);
        
        // When: The CancelBookingAsync method is called
        var result = _bookingService.CancelBookingAsync(bookingId).Result;

        // Then: Expect the booking to not be cancelled, because it is already completed
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Booking is already completed", result.ErrorMEssage);
        _bookingRepoMock.Verify(repo => repo.GetByIdAsync(bookingId), Times.Once);
    }

    [TestMethod]
    public void CreateBookingAsync_ShouldAddBooking_WhenValidBooking()
    {
        // Given: A valid booking
        var booking = new Booking
        {
            Id = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            NumberOfPeople = 2
        };
        _bookingRepoMock.Setup(repo => repo.AddAsync(booking)).Returns(Task.CompletedTask);
        _bookingRepoMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
        
        // When: The CreateBookingAsync method is called
        var result = _bookingService.CreateBookingAsync(booking).Result;
        
        // Then: Expect the booking to be added and saved
        Assert.AreEqual(booking, result);
        _bookingRepoMock.Verify(repo => repo.AddAsync(booking), Times.Once);
        _bookingRepoMock.Verify(repo => repo.SaveAsync(), Times.Once);
    }
}
