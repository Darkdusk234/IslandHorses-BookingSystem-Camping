using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;
using Camping_BookingSystem.Repositories;
using Camping_BookingSystem.Services;
using Camping_BookingSystem.Services.BookingServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Camping_BookingSystemTests;
/*The BookingService file became rather messy, 
 * so i created a validator class instead. 
 * 
 * So the tests in this file test both BookingService.cs and BookingValidator.cs*/


[TestClass]
public class BookingServiceTests_MOQ
{
    private Mock<IBookingRepository> _bookingRepoMock;
    private Mock<ICampSpotRepository> _campSpotRepoMock;
    private Mock<ICustomerRepository> _customerRepoMock;
    private Mock<IBookingValidator> _bookingValidatorMock;
    private Mock<ISpotTypeRepository> _spotTypeRepoMock;
    private BookingService _bookingService;

    [TestInitialize]
    public void Setup()
    {
        _bookingRepoMock = new Mock<IBookingRepository>();
        _campSpotRepoMock = new Mock<ICampSpotRepository>();
        _customerRepoMock = new Mock<ICustomerRepository>();
        _bookingValidatorMock = new Mock<IBookingValidator>();
        _spotTypeRepoMock = new Mock<ISpotTypeRepository>();

        _bookingService = new BookingService(
            _bookingValidatorMock.Object,
            _bookingRepoMock.Object,
            _campSpotRepoMock.Object,
            _customerRepoMock.Object,
            _spotTypeRepoMock.Object
        );
    }
    [TestMethod]
    public async Task CancelBookingAsync_ShouldCancel_WhenValidStatus()
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
        var result = await _bookingService.CancelBookingAsync(bookingId);

        // Then: Expect the booking status to be changed to Cancelled
        Assert.IsTrue(result.Success);
        Assert.IsNull(result.ErrorMEssage);
        Assert.AreEqual(BookingStatus.Cancelled, booking.Status);
        _bookingRepoMock.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [TestMethod]
    public async Task CancelBookingAsync_ShouldReturnError_WhenBookingNotFound()
    {
        // Given: A booking ID that does not exist
        var bookingId = 999;
        _bookingRepoMock.Setup(repo => repo.GetByIdAsync(bookingId)).ReturnsAsync((Booking?)null);

        // When: The CancelBookingAsync method is called
        var result = await _bookingService.CancelBookingAsync(bookingId);

        // Then: Expect the booking to not be cancelled, because it does not exist
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Booking not found", result.ErrorMEssage);
        _bookingRepoMock.Verify(repo => repo.GetByIdAsync(bookingId), Times.Once);
    }

    [TestMethod]
    public async Task CancelBookingAsync_ShouldReturnError_WhenBookingAlreadyCancelled()
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
        var result = await _bookingService.CancelBookingAsync(bookingId);
        
        // Then: Expect the booking to not be cancelled, because it is already cancelled
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Booking is already cancelled", result.ErrorMEssage);
        _bookingRepoMock.Verify(repo => repo.GetByIdAsync(bookingId), Times.Once);
    }

    [TestMethod]
    public async Task CancelBookingAsync_ShouldReturnError_WhenBookingAlreadyCompleted()
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
        var result = await _bookingService.CancelBookingAsync(bookingId);

        // Then: Expect the booking to not be cancelled, because it is already completed
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Booking is already completed", result.ErrorMEssage);
        _bookingRepoMock.Verify(repo => repo.GetByIdAsync(bookingId), Times.Once);
    }

    [TestMethod]
    public async Task CreateBookingAsync_ShouldAddBooking_WhenCalledWithValidBooking()
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

    [TestMethod]
    public async Task CreateBookingWithCustomerAsync_ShouldReturnBadRequest_WhenStartDateInPast()
    {
        // Given: A booking request with a start date in the past
        var request = new CreateBookingAndCustomer
        { 
            StartDate = DateTime.Today.AddDays(-1),
            EndDate = DateTime.Today.AddDays(2),
        };

        _bookingValidatorMock
            .Setup(v => v.ValidateCreateAsync(request))
            .Throws(new ArgumentException("Start date cannot be in the past."));

        // When: The CreateBookingWithCustomerAsync method is called
        // Then: Expect an ArgumentException to be thrown
        var errorMessage = 
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => 
            _bookingService.CreateBookingWithCustomerAsync(request));
        Assert.AreEqual("Start date cannot be in the past.", errorMessage.Message);
    }

    [TestMethod]
    public async Task CreateBookingWithCustomerAsync_ShouldThrow_WhenEndDateBeforeStartDate()
    {
        // Given: A booking request with an end date before the start date
        var request = new CreateBookingAndCustomer
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(-2),
        };

        _bookingValidatorMock
            .Setup(v => v.ValidateCreateAsync(request))
            .Throws(new ArgumentException("End date must be after start date."));

        // When: The CreateBookingWithCustomerAsync method is called
        // Then: Expect an ArgumentException to be thrown
        var errorMessage = 
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => 
            _bookingService.CreateBookingWithCustomerAsync(request));

        Assert.AreEqual("End date must be after start date.", errorMessage.Message);
    }


    [TestMethod]
    public async Task CreateBookingWithCustomerAsync_ShouldReturnBadRequest_WhenNumberOfPeopleExceedsLimit()
    {
        // Given: A booking request with a number of people exceeding the limit
        
        var request = new CreateBookingAndCustomer
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(2),
            NumberOfPeople = 11, // Exceeds the limit of 10
        };
        _bookingValidatorMock
            .Setup(v => v.ValidateCreateAsync(request))
            .ReturnsAsync((false, "Number of people must be between 1 and 10."));
        // When: The CreateBookingWithCustomerAsync method is called
        // Then: Expect an ArgumentException to be thrown
        var result =
            await _bookingService.CreateBookingWithCustomerAsync(request);

        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("Number of people must be between 1 and 10.", badRequestResult.Value);
    }

    [TestMethod]
    public async Task ValidateCreateAsync_ShouldReturnBadRequest_WhenCampSpotNotAvailable()
    {
        // Given: A booking request for a camp spot that is not available
        var request = new CreateBookingAndCustomer
        {
            StartDate = DateTime.Today.AddDays(5),
            EndDate = DateTime.Today.AddDays(8),
            CampSpotId = 1,
            NumberOfPeople = 2,
        };
        
        var existingBooking = new Booking
        {
            CampSpotId = 1,
            StartDate = DateTime.Today.AddDays(6),
            EndDate = DateTime.Today.AddDays(7),
            NumberOfPeople = 2
        };
        _campSpotRepoMock
            .Setup(repo => repo.GetCampSpotById(request.CampSpotId))
            .ReturnsAsync(new CampSpot { 
                Id = request.CampSpotId, 
                SpotType = new SpotType 
                { MaxPersonLimit = 10 } });


        _bookingRepoMock.Setup(r =>
        r.GetBookingsByCampSpotAndDate(
            request.CampSpotId, 
            request.StartDate, 
            request.EndDate))
            .ReturnsAsync(new List<Booking> {existingBooking});

        // When: The ValidateCreateAsync method is called
        var validator = new BookingValidator(_campSpotRepoMock.Object, _bookingRepoMock.Object);

        var (isValid, errorMessage) = 
            await validator.ValidateCreateAsync(request);
        // Then: Expect an error message to be thrown and isValid to be false
        Assert.IsFalse(isValid);
        Assert.AreEqual("Camp spot is not available for the selected dates.", errorMessage);
        _bookingRepoMock.Verify(repo => repo.GetBookingsByCampSpotAndDate(request.CampSpotId, request.StartDate, request.EndDate), Times.Once);
    }

    [TestMethod]
    public async Task UpdateBookingAddOnsAsync_ShouldUpdateAddOns_WhenValidRequest()
    {
        // Given: A valid booking ID and request to update add-ons
        var bookingId = 1;
        var request = new UpdateAddonsRequest
        {
            Wifi = true,
            Parking = true
        };
        
        var booking = new Booking
        {
            Id = bookingId,
            Status = BookingStatus.Pending,
            Wifi = false,
            Parking = false
        };
        
        _bookingRepoMock.Setup(repo => repo.GetByIdAsync(bookingId)).ReturnsAsync(booking);
        _bookingRepoMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
        // When: The UpdateBookingAddOnsAsync method is called
        var result = await _bookingService.UpdateBookingAddOnsAsync(bookingId, request);
        // Then: Expect the add-ons to be updated successfully
        Assert.IsTrue(result.Success);
        Assert.IsNull(result.ErrorMessage);
        Assert.IsTrue(booking.Wifi);
        Assert.IsTrue(booking.Parking);
        
        _bookingRepoMock.Verify(repo => repo.Update(booking), Times.Once);
        _bookingRepoMock.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [TestMethod]
    public async Task UpdateBookingAddOnsAsync_ShouldReturnError_WhenBookingNotFound()
    {
        // Given: A booking ID that does not exist
        var bookingId = 999;
        var request = new UpdateAddonsRequest
        {
            Wifi = true,
            Parking = true
        };
        
        _bookingRepoMock.Setup(repo => repo.GetByIdAsync(bookingId)).ReturnsAsync((Booking?)null);
        // When: The UpdateBookingAddOnsAsync method is called
        var result = await _bookingService.UpdateBookingAddOnsAsync(bookingId, request);
        // Then: Expect an error message to be returned
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Booking not found", result.ErrorMessage);
        _bookingRepoMock.Verify(repo => repo.GetByIdAsync(bookingId), Times.Once);
    }

    [TestMethod]
    public async Task UpdateBookingAddOnsAsync_ShouldReturnError_WhenBookingAlreadyCompleted()
    {
        // Given: A booking that is already completed
        var bookingId = 1;
        var request = new UpdateAddonsRequest
        {
            Wifi = true,
            Parking = true
        };
        
        var booking = new Booking
        {
            Id = bookingId,
            Status = BookingStatus.Completed
        };
        
        _bookingRepoMock.Setup(repo => repo.GetByIdAsync(bookingId)).ReturnsAsync(booking);
        
        // When: The UpdateBookingAddOnsAsync method is called
        var result = await _bookingService.UpdateBookingAddOnsAsync(bookingId, request);
        
        // Then: Expect an error message to be returned
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Booking can not be updated, it is already completed.", result.ErrorMessage);
        _bookingRepoMock.Verify(repo => repo.GetByIdAsync(bookingId), Times.Once);
    }

    [TestMethod]
    public async Task UpdateBookingAddOnsAsync_ShouldReturnError_WhenBookingAlreadyCancelled()
    {
        // Given: A booking that is already cancelled
        var bookingId = 1;
        var request = new UpdateAddonsRequest
        {
            Wifi = true,
            Parking = true
        };
        
        var booking = new Booking
        {
            Id = bookingId,
            Status = BookingStatus.Cancelled
        };
        
        _bookingRepoMock.Setup(repo => repo.GetByIdAsync(bookingId)).ReturnsAsync(booking);
        
        // When: The UpdateBookingAddOnsAsync method is called
        var result = await _bookingService.UpdateBookingAddOnsAsync(bookingId, request);
        
        // Then: Expect an error message to be returned
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Booking can not be updated, it is already cancelled.", result.ErrorMessage);
        _bookingRepoMock.Verify(repo => repo.GetByIdAsync(bookingId), Times.Once);
    }

    [TestMethod]
    public async Task IsCampSpotAvailableAsync_ShouldReturnTrue_WhenCampSpotIsAvailable()
    {
        // Given: A camp spot that is available for the requested dates
        var campSpotId = 1;
        var startDate = DateTime.Today.AddDays(1);
        var endDate = DateTime.Today.AddDays(3);
        var numberOfPeople = 2;
        _campSpotRepoMock.Setup(repo => repo.GetCampSpotById(campSpotId))
            .ReturnsAsync(new CampSpot { Id = campSpotId, SpotType = new SpotType { MaxPersonLimit = 10 } });
        _bookingRepoMock.Setup(repo => repo.GetBookingsByCampSpotAndDate(campSpotId, startDate, endDate))
            .ReturnsAsync(new List<Booking>());
        // When: The IsCampSpotAvailableAsync method is called
        var result = await _bookingService.IsCampSpotAvailableAsync(campSpotId, startDate, endDate, numberOfPeople);
        // Then: Expect the camp spot to be available
        Assert.IsTrue(result.IsAvailable);
        Assert.IsNull(result.Reason);
    }

    [TestMethod]
    public async Task IsCampSpotAvailableAsync_ShouldReturnFalse_WhenCampSpotIsNotAvailable()
    {
        // Given: A camp spot that is not available for the requested dates
        var campSpotId = 1;
        var typeId = 1;
        var startDate = DateTime.Today.AddDays(1);
        var endDate = DateTime.Today.AddDays(3);
        var numberOfPeople = 2;
        
        _campSpotRepoMock.Setup(repo => repo.GetCampSpotById(campSpotId))
            .ReturnsAsync(new CampSpot { Id = campSpotId, TypeId = typeId });
        _spotTypeRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new SpotType { MaxPersonLimit = 10 });
        
        var existingBooking = new Booking
        {
            CampSpotId = campSpotId,
            StartDate = startDate.AddDays(1),
            EndDate = endDate.AddDays(1),
            NumberOfPeople = 2
        };
        
        _bookingRepoMock.Setup(repo => repo.GetBookingsByCampSpotAndDate(campSpotId, startDate, endDate))
            .ReturnsAsync(new List<Booking> { existingBooking });
        
        // When: The IsCampSpotAvailableAsync method is called
        var result = await _bookingService.IsCampSpotAvailableAsync(campSpotId, startDate, endDate, numberOfPeople);
        
        // Then: Expect the camp spot to not be available
        Assert.IsFalse(result.IsAvailable);
        Assert.AreEqual("Camp spot is not available for the selected dates.", result.Reason);
    }

    [TestMethod]
    public async Task IsCampSpotAvailableAsync_ShouldReturnFalse_WhenStartDateInPast()
    {
        // Given: A start date in the past
        var campSpotId = 1;
        var startDate = DateTime.Today.AddDays(-1);
        var endDate = DateTime.Today.AddDays(1);
        var numberOfPeople = 2;
        // When: The IsCampSpotAvailableAsync method is called
        var result = await _bookingService.IsCampSpotAvailableAsync(campSpotId, startDate, endDate, numberOfPeople);
        // Then: Expect the camp spot to not be available
        Assert.IsFalse(result.IsAvailable);
        Assert.AreEqual("Start date cannot be in the past.", result.Reason);
    }

    [TestMethod]
    public async Task IsCampSpotAvailableAsync_ShouldReturnFalse_WhenEndDateBeforeStartDate()
    {
        // Given: An end date before the start date
        var campSpotId = 1;
        var startDate = DateTime.Today.AddDays(2);
        var endDate = DateTime.Today.AddDays(1);
        var numberOfPeople = 2;
        // When: The IsCampSpotAvailableAsync method is called
        var result = await _bookingService.IsCampSpotAvailableAsync(campSpotId, startDate, endDate, numberOfPeople);
        // Then: Expect the camp spot to not be available
        Assert.IsFalse(result.IsAvailable);
        Assert.AreEqual("End date must be after start date.", result.Reason);
    }

    [TestMethod]
    public async Task IsCampSpotAvailableAsync_ShouldReturnFalse_WhenNumberOfPeopleExceedsLimit()
    {
        // Given: A number of people that exceeds the camp spot's limit
        var campSpotId = 1;
        var startDate = DateTime.Today.AddDays(1);
        var endDate = DateTime.Today.AddDays(3);
        var numberOfPeople = 15; // Exceeds the limit of 10
        
        _campSpotRepoMock.Setup(repo => repo.GetCampSpotById(campSpotId))
            .ReturnsAsync(new CampSpot { Id = campSpotId, SpotType = new SpotType { MaxPersonLimit = 10 } });
        
        // When: The IsCampSpotAvailableAsync method is called
        var result = await _bookingService.IsCampSpotAvailableAsync(campSpotId, startDate, endDate, numberOfPeople);
        
        // Then: Expect the camp spot to not be available
        Assert.IsFalse(result.IsAvailable);
        Assert.AreEqual("Camp spot can not accommodate the number of people.", result.Reason);
    }

    [TestMethod]
    public async Task ValidateDeleteAsync_ShouldReturnTrue_WhenBookingIsValidForDeletion()
    {
        // Given: A booking that is valid for deletion
        var bookingId = 1;
        var booking = new Booking
        {
            Id = bookingId,
            Status = BookingStatus.Pending
        };
        
        _bookingRepoMock.Setup(repo => repo.GetByIdAsync(bookingId)).ReturnsAsync(booking);
        var validator = new BookingValidator(_campSpotRepoMock.Object, _bookingRepoMock.Object);
        // When: The ValidateDeleteAsync method is called
        var result = await validator.ValidateDeleteAsync(bookingId);
        
        // Then: Expect the booking to be valid for deletion
        Assert.IsTrue(result.IsValid);
        Assert.IsNull(result.ErrorMessage);
        _bookingRepoMock.Verify(repo => repo.GetByIdAsync(bookingId), Times.Once);
    }

}
