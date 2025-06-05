using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.SpotTypeDTOs;
using Camping_BookingSystem.Controllers;
using Camping_BookingSystem.Services;
using Camping_BookingSystem.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
namespace Camping_BookingSystemTests;

[TestClass]
public class SpotTypeControllerTests
{
    private Mock<ISpotTypeService> _mockSpotTypeService;
    private SpotTypeController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockSpotTypeService = new Mock<ISpotTypeService>();
        _controller = new SpotTypeController(_mockSpotTypeService.Object);
    }
    [TestCleanup]
    public void Cleanup()
    {
        _mockSpotTypeService = null;
        _controller = null;
    }

    // ------------------------------------------ GetAllAsync Tests ------------------------------------------
    [TestMethod]
    public async Task GetAllSpotTypes_ReturnsOkResult_WithSpotTypeDetails()
    {
        // Arrange
        var expectedSpotTypes = new List<SpotType>
            {
                new SpotType { Id = 1, Name = "Tent Site", Price = 150.00m, MaxPersonLimit = 4 },
                new SpotType { Id = 2, Name = "RV Site", Price = 300.00m, MaxPersonLimit = 6 }
            };

        _mockSpotTypeService
            .Setup(s => s.GetAllSpotTypesAsync())
            .ReturnsAsync(expectedSpotTypes);

        // Act
        var result = await _controller.GetAllSpotTypes();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        var actualSpotTypes = (IEnumerable<SpotTypeDetailsResponse>)okResult.Value;
        Assert.AreEqual(expectedSpotTypes.Count, actualSpotTypes.Count());

        var firstSpotType = actualSpotTypes.First();
        Assert.AreEqual(expectedSpotTypes.First().Id, firstSpotType.Id);
        Assert.AreEqual(expectedSpotTypes.First().Name, firstSpotType.Name);
        Assert.AreEqual(expectedSpotTypes.First().Price, firstSpotType.Price);

        _mockSpotTypeService.Verify(s => s.GetAllSpotTypesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetAllSpotTypes_EmptyList_ReturnsOkWithEmptyResult()
    {
        // Arrange
        var emptySpotTypes = new List<SpotType>();

        _mockSpotTypeService
            .Setup(s => s.GetAllSpotTypesAsync())
            .ReturnsAsync(emptySpotTypes);

        // Act
        var result = await _controller.GetAllSpotTypes();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        var actualSpotTypes = (IEnumerable<SpotTypeDetailsResponse>)okResult.Value;
        Assert.AreEqual(0, actualSpotTypes.Count());
    }

    [TestMethod]
    public async Task GetAllSpotTypes_ServiceThrowsException_PropagatesException()
    {
        // Arrange
        _mockSpotTypeService
            .Setup(s => s.GetAllSpotTypesAsync())
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetAllSpotTypes());
    }

    // ------------------------------------------ GetAllAsync Tests ------------------------------------------

    // ------------------------------------------ GetByIdAsync Tests ------------------------------------------

    [TestMethod]
    public async Task GetSpotTypeById_ValidId_ReturnsOkWithMappedResult()
    {
        // Arrange
        var spotTypeId = 1;
        var expectedSpotType = new SpotType
        {
            Id = spotTypeId,
            Name = "Tent Site",
            Price = 150.00m,
            MaxPersonLimit = 4
        };

        _mockSpotTypeService
            .Setup(s => s.GetSpotTypeByIdAsync(spotTypeId))
            .ReturnsAsync(expectedSpotType);

        // Act
        var result = await _controller.GetSpotTypeById(spotTypeId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        var actualSpotType = (SpotTypeDetailsResponse)okResult.Value;
        Assert.AreEqual(expectedSpotType.Id, actualSpotType.Id);
        Assert.AreEqual(expectedSpotType.Name, actualSpotType.Name);
        Assert.AreEqual(expectedSpotType.Price, actualSpotType.Price);
        Assert.AreEqual(expectedSpotType.MaxPersonLimit, actualSpotType.MaxPersonLimit);
    }

    [TestMethod]
    public async Task GetSpotTypeById_SpotTypeNotFound_ReturnsNotFound()
    {
        // Arrange
        var spotTypeId = 999;
        _mockSpotTypeService
            .Setup(s => s.GetSpotTypeByIdAsync(spotTypeId))
            .ReturnsAsync((SpotType?)null);

        // Act
        var result = await _controller.GetSpotTypeById(spotTypeId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(42)]
    [DataRow(100)]
    public async Task GetSpotTypeById_DifferentValidIds_CallsServiceWithCorrectId(int testId)
    {
        // Arrange
        var spotType = new SpotType { Id = testId, Name = "Test Type", Price = 100m, MaxPersonLimit = 2 };
        _mockSpotTypeService.Setup(s => s.GetSpotTypeByIdAsync(testId)).ReturnsAsync(spotType);

        // Act
        await _controller.GetSpotTypeById(testId);

        // Assert
        _mockSpotTypeService.Verify(s => s.GetSpotTypeByIdAsync(testId), Times.Once);
    }

    // ------------------------------------------ GetByIdAsync Tests ------------------------------------------

    // ------------------------------------------ CreateSpotType Tests ------------------------------------------

    [TestMethod]
    public async Task CreateSpotType_ValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        var request = new CreateSpotTypeRequest
        {
            Name = "Epic Royal Tent",
            Price = 175.00m,
            MaxPersonLimit = 5
        };

        var createdSpotType = new SpotType
        {
            Id = 1,
            Name = request.Name,
            Price = request.Price,
            MaxPersonLimit = request.MaxPersonLimit
        };

        _mockSpotTypeService
            .Setup(s => s.CreateSpotTypeAsync(It.IsAny<SpotType>()))
            .ReturnsAsync(createdSpotType)
            .Callback<SpotType>(st => st.Id = 1); 

        // Act
        var result = await _controller.CreateSpotType(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        var createdAtActionResult = (CreatedAtActionResult)result;
        Assert.AreEqual(nameof(SpotTypeController.GetSpotTypeById), createdAtActionResult.ActionName);
        Assert.AreEqual(createdSpotType.Id, createdAtActionResult.RouteValues!["id"]);

        var returnedSpotType = (SpotTypeDetailsResponse)createdAtActionResult.Value;
        Assert.AreEqual(createdSpotType.Name, returnedSpotType.Name);
        Assert.AreEqual(createdSpotType.Price, returnedSpotType.Price);

        // Verify service was called with correctly mapped SpotType
        _mockSpotTypeService.Verify(s => s.CreateSpotTypeAsync(It.Is<SpotType>(st =>
            st.Name == request.Name &&
            st.Price == request.Price &&
            st.MaxPersonLimit == request.MaxPersonLimit)), Times.Once);
    }

    [TestMethod]
    public async Task CreateSpotType_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateSpotTypeRequest(); // Invalid request, missing required fields
        _controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _controller.CreateSpotType(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.IsInstanceOfType(badRequestResult.Value, typeof(SerializableError));

        _mockSpotTypeService.Verify(s => s.CreateSpotTypeAsync(It.IsAny<SpotType>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateSpotType_ServiceThrowsArgumentNullException_PropagatesException()
    {
        // Arrange
        var request = new CreateSpotTypeRequest
        {
            Name = "Test Type",
            Price = 100m,
            MaxPersonLimit = 2
        };

        _mockSpotTypeService
            .Setup(s => s.CreateSpotTypeAsync(It.IsAny<SpotType>()))
            .ThrowsAsync(new ArgumentNullException("spotType"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _controller.CreateSpotType(request));
    }

    [TestMethod]
    public async Task CreateSpotType_ServiceThrowsArgumentException_PropagatesException()
    {
        // Arrange
        var request = new CreateSpotTypeRequest
        {
            Name = "", 
            Price = 100m,
            MaxPersonLimit = 2
        };

        _mockSpotTypeService
            .Setup(s => s.CreateSpotTypeAsync(It.IsAny<SpotType>()))
            .ThrowsAsync(new ArgumentException("Name cannot be empty"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.CreateSpotType(request));
    }

    // ------------------------------------------ CreateSpotType Tests ------------------------------------------

    // ------------------------------------------ UpdateSpotType Tests ------------------------------------------
    [TestMethod]
    public async Task UpdateSpotType_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var spotTypeId = 1;
        var request = new UpdateSpotTypeRequest
        {
            Id = spotTypeId,
            Name = "Updated Tent Site",
            Price = 200.00m,
            MaxPersonLimit = 6
        };

        var existingSpotType = new SpotType
        {
            Id = spotTypeId,
            Name = "Old Tent Site",
            Price = 150.00m,
            MaxPersonLimit = 4
        };

        _mockSpotTypeService
            .Setup(s => s.GetSpotTypeByIdAsync(spotTypeId))
            .ReturnsAsync(existingSpotType);

        _mockSpotTypeService
            .Setup(s => s.UpdateSpotTypeAsync(It.IsAny<SpotType>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateSpotType(spotTypeId, request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));

        // Verify service calls
        _mockSpotTypeService.Verify(s => s.GetSpotTypeByIdAsync(spotTypeId), Times.Once);
        _mockSpotTypeService.Verify(s => s.UpdateSpotTypeAsync(It.Is<SpotType>(st =>
            st.Id == spotTypeId &&
            st.Name == request.Name &&
            st.Price == request.Price &&
            st.MaxPersonLimit == request.MaxPersonLimit)), Times.Once);
    }

    [TestMethod]
    public async Task UpdateSpotType_IdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var spotTypeId = 1;
        var request = new UpdateSpotTypeRequest
        {
            Id = 2, // Different ID causes mismatch
            Name = "Updated Type",
            Price = 200.00m,
            MaxPersonLimit = 6
        };

        // Act
        var result = await _controller.UpdateSpotType(spotTypeId, request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.AreEqual("No ID for that type", badRequestResult.Value); 

        // Verify service was never called
        _mockSpotTypeService.Verify(s => s.GetSpotTypeByIdAsync(It.IsAny<int>()), Times.Never);
        _mockSpotTypeService.Verify(s => s.UpdateSpotTypeAsync(It.IsAny<SpotType>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateSpotType_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var spotTypeId = 1;
        var request = new UpdateSpotTypeRequest
        {
            Id = spotTypeId,
            Name = "",
            Price = 200.00m,
            MaxPersonLimit = 6
        };
        _controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _controller.UpdateSpotType(spotTypeId, request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.IsInstanceOfType(badRequestResult.Value, typeof(SerializableError));
    }

    [TestMethod]
    public async Task UpdateSpotType_SpotTypeNotFound_ReturnsNotFound()
    {
        // Arrange
        var spotTypeId = 999;
        var request = new UpdateSpotTypeRequest
        {
            Id = spotTypeId,
            Name = "Updated Type",
            Price = 200.00m,
            MaxPersonLimit = 6
        };

        _mockSpotTypeService
            .Setup(s => s.GetSpotTypeByIdAsync(spotTypeId))
            .ReturnsAsync((SpotType?)null);

        // Act
        var result = await _controller.UpdateSpotType(spotTypeId, request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        // Verify update was never called
        _mockSpotTypeService.Verify(s => s.UpdateSpotTypeAsync(It.IsAny<SpotType>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateSpotType_ServiceThrowsException_PropagatesException()
    {
        // Arrange
        var spotTypeId = 1;
        var request = new UpdateSpotTypeRequest
        {
            Id = spotTypeId,
            Name = "Updated Type",
            Price = 200.00m,
            MaxPersonLimit = 6
        };

        var existingSpotType = new SpotType { Id = spotTypeId, Name = "Old Type" };

        _mockSpotTypeService.Setup(s => s.GetSpotTypeByIdAsync(spotTypeId)).ReturnsAsync(existingSpotType);
        _mockSpotTypeService.Setup(s => s.UpdateSpotTypeAsync(It.IsAny<SpotType>()))
            .ThrowsAsync(new ArgumentNullException("spotType"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _controller.UpdateSpotType(spotTypeId, request));
    }
    // ------------------------------------------ UpdateSpotType Tests ------------------------------------------

    // ------------------------------------------ DeleteSpotType Tests ------------------------------------------
    [TestMethod]
    public async Task DeleteSpotType_ValidId_ReturnsNoContent()
    {
        // Arrange
        var spotTypeId = 1;
        var existingSpotType = new SpotType
        {
            Id = spotTypeId,
            Name = "Type to Delete",
            Price = 150.00m,
            MaxPersonLimit = 4
        };

        _mockSpotTypeService
            .Setup(s => s.GetSpotTypeByIdAsync(spotTypeId))
            .ReturnsAsync(existingSpotType);

        _mockSpotTypeService
            .Setup(s => s.DeleteSpotTypeAsync(spotTypeId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteSpotType(spotTypeId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));

        // Verify service calls
        _mockSpotTypeService.Verify(s => s.GetSpotTypeByIdAsync(spotTypeId), Times.Once);
        _mockSpotTypeService.Verify(s => s.DeleteSpotTypeAsync(spotTypeId), Times.Once);
    }

    [TestMethod]
    public async Task DeleteSpotType_SpotTypeNotFound_ReturnsNotFound()
    {
        // Arrange
        var spotTypeId = 999;
        _mockSpotTypeService
            .Setup(s => s.GetSpotTypeByIdAsync(spotTypeId))
            .ReturnsAsync((SpotType?)null);

        // Act
        var result = await _controller.DeleteSpotType(spotTypeId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        // Verify delete was never called
        _mockSpotTypeService.Verify(s => s.DeleteSpotTypeAsync(It.IsAny<int>()), Times.Never);
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(42)]
    [DataRow(100)]
    public async Task DeleteSpotType_DifferentValidIds_CallsServiceWithCorrectId(int testId)
    {
        // Arrange
        var spotType = new SpotType { Id = testId, Name = "Test Type" };
        _mockSpotTypeService.Setup(s => s.GetSpotTypeByIdAsync(testId)).ReturnsAsync(spotType);
        _mockSpotTypeService.Setup(s => s.DeleteSpotTypeAsync(testId)).Returns(Task.CompletedTask);

        // Act
        await _controller.DeleteSpotType(testId);

        // Assert
        _mockSpotTypeService.Verify(s => s.DeleteSpotTypeAsync(testId), Times.Once);
    }

    [TestMethod]
    public async Task DeleteSpotType_ServiceThrowsException_PropagatesException()
    {
        // Arrange
        var spotTypeId = 1;
        var existingSpotType = new SpotType { Id = spotTypeId, Name = "Test Type" };

        _mockSpotTypeService.Setup(s => s.GetSpotTypeByIdAsync(spotTypeId)).ReturnsAsync(existingSpotType);
        _mockSpotTypeService.Setup(s => s.DeleteSpotTypeAsync(spotTypeId))
            .ThrowsAsync(new InvalidOperationException("Cannot delete spot type with active bookings"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await _controller.DeleteSpotType(spotTypeId));
    }

    // ------------------------------------------ DeleteSpotType Tests ------------------------------------------
}
