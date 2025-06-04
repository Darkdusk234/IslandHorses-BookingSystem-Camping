using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs;
using BookingSystem_ClassLibrary.Models.DTOs.CampSiteDTO;
using Camping_BookingSystem.Controllers;
using Camping_BookingSystem.Services;
using Camping_BookingSystem.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Camping_BookingSystemTests;

[TestClass]
public class CampSiteControllerTests
{
    private Mock<ICampSiteService> _mockCampSite;
    private CampsiteController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockCampSite = new Mock<ICampSiteService>();
        _controller = new CampsiteController(_mockCampSite.Object);  // Fixed: _ instead of *
    }

    [TestCleanup]
    public void Cleanup()
    {
        //_controller?.Dispose();
        _mockCampSite = null;
        _controller = null;
    }

    // ------------------------------------------ GetAllAsync Tests ------------------------------------------

    [TestMethod]
    public async Task GetAllCampSites_ReturnsOkResult_WithCampSiteDetails()
    {
        // Arrange
        var expectedCampSites = new List<CampSite>
        {
            new CampSite { Id = 1, Name = "Horse Camp", Description = "A horse spot", Adress = "123 Camp St" },
            new CampSite { Id = 2, Name = "Forest Camp", Description = "Peaceful forest location", Adress = "456 Forest Ave" }
        };
        _mockCampSite
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(expectedCampSites);
        // Act
        var result = await _controller.GetAllCampSites();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        var actualCampSites = (IEnumerable<CampSiteDetailsResponse>)okResult.Value;
        Assert.AreEqual(expectedCampSites.Count, actualCampSites.Count());

        // Verify mapping worked correctly
        var firstCampSite = actualCampSites.First();
        Assert.AreEqual(expectedCampSites.First().Id, firstCampSite.Id);
        Assert.AreEqual(expectedCampSites.First().Name, firstCampSite.Name);

        // Verify service was called once
        _mockCampSite.Verify(s => s.GetAllAsync(), Times.Once);
    }
    [TestMethod]
    public async Task GetAllCampSites_EmptyList_ReturnsOkWithEmptyResult()
    {
        // Arrange
        var noCampSites = new List<CampSite>();

        _mockCampSite
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(noCampSites);
        // Act
        var result = await _controller.GetAllCampSites();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        var actualCampSites = (IEnumerable<CampSiteDetailsResponse>)okResult.Value;
        Assert.AreEqual(0, actualCampSites.Count());

    }
    [TestMethod]
    public async Task GetAllCampSites_ServiceThrowsException_PropagatesExecption()
    {
        // Arrange
        _mockCampSite
            .Setup(s => s.GetAllAsync())
            .ThrowsAsync(new Exception("Service error"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.GetAllCampSites());
    }

    // ------------------------------------------ GetAllAsync Tests ------------------------------------------

    // ------------------------------------------ GetByIdAsync Tests ------------------------------------------

    [TestMethod]
    public async Task GetCampSiteById_ValidId_ReturnsOkResultat()
    {
        // Arrange
        var campSiteId = 1;
        var expectedCampSite = new CampSite { Id = campSiteId, Name = "Horse Camp", Description = "A horse spot", Adress = "123 Camp St" };

        _mockCampSite
            .Setup(s => s.GetByIdAsync(campSiteId))
            .ReturnsAsync(expectedCampSite);

        // Act
        var result = await _controller.GetCampSiteById(campSiteId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        var actualCampSite = (CampSiteDetailsResponse)okResult.Value;
        Assert.AreEqual(expectedCampSite.Id, actualCampSite.Id);
        Assert.AreEqual(expectedCampSite.Name, actualCampSite.Name);

    }

    [TestMethod]
    public async Task GetCampSiteById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 666;
        _mockCampSite
            .Setup(s => s.GetByIdAsync(invalidId))
            .ReturnsAsync((CampSite?)null);

        // Act
        var result = await _controller.GetCampSiteById(invalidId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        var notFoundResult = (NotFoundObjectResult)result;
        var expectedMessage = $"Camping plats med id: {invalidId}, finns inte, Skriv in rätt nästa gång.";
        Assert.AreEqual(expectedMessage, notFoundResult.Value);
    }

    // ------------------------------------------ GetByIdAsync Tests ------------------------------------------

    // ------------------------------------------ CreateAsync Tests ------------------------------------------

    [TestMethod]
    public async Task AddCampSite_ValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        var newCampSite = new CreateCampSiteRequest
        {
            Name = "New Horse Camp",
            Description = "A new  Horse camping site",
            Adress = "789 New Horse Rd"
        };

        var createdCampSite = new CampSite
        {
            Id = 1,
            Name = newCampSite.Name,
            Description = newCampSite.Description,
            Adress = newCampSite.Adress
        };

        _mockCampSite
            .Setup(s => s.CreateAsync(It.IsAny<CampSite>()))
            .ReturnsAsync(createdCampSite)
            .Callback<CampSite>(cs => cs.Id = 1);  

        // Act
        var result = await _controller.AddCampSite(newCampSite);
        // Assert
        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        var createdResult = (CreatedAtActionResult)result;
        var actualCampSite = (CampSiteDetailsResponse)createdResult.Value;
        Assert.AreEqual(createdCampSite.Id, actualCampSite.Id);
        Assert.AreEqual(createdCampSite.Name, actualCampSite.Name);
    }

    [TestMethod]
    public async Task AddCampSite_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateCampSiteRequest
        {
            Name = "",
            Description = "A new  Horse camping site",
            Adress = "789 New Horse Rd"
        };
        _controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _controller.AddCampSite(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.IsTrue(badRequestResult.Value is SerializableError);
    }

    [TestMethod]
    public async Task AddCampSite_ServiceThrowsArgumentNullExecption_Exception()
    {
        // Arrange
        var request = new CreateCampSiteRequest
        {
            Name = "Horse Camp 2",
            Description = "There are Horses",
            Adress = "Horse Road 1"
        };
        _mockCampSite
            .Setup(s => s.CreateAsync(It.IsAny<CampSite>()))
            .ThrowsAsync(new ArgumentNullException("campSite"));
        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _controller.AddCampSite(request));

    }

    [TestMethod]
    public async Task AddCampSite_ServiceThrowsArgumentException_PropagatesException()
    {
        // Arrange
        var request = new CreateCampSiteRequest
        {
            Name = "",
            Description = "Test Description",
            Adress = "Test Address"
        };

        _mockCampSite
            .Setup(s => s.CreateAsync(It.IsAny<CampSite>()))
            .ThrowsAsync(new ArgumentException("Name cannot be empty or whitespace"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _controller.AddCampSite(request));
    }
    // ------------------------------------------ CreateAsync Tests ------------------------------------------

    // ------------------------------------------ UpdateAsync Tests ------------------------------------------
    [TestMethod]
    public async Task UpdateCampSite_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var campSiteId = 1;
        var updateRequest = new UpdateCampSiteRequest
        {
            Id = campSiteId,
            Name = "Updated Horse Camp",
            Description = "Updated description",
            Adress = "Updated Address"
        };

        var updatedCampSite = new CampSite
        {
            Id = campSiteId,
            Name = updateRequest.Name,
            Description = updateRequest.Description,
            Adress = updateRequest.Adress
        };
        _mockCampSite
            .Setup(s => s.GetByIdAsync(campSiteId))
            .ReturnsAsync(updatedCampSite);

        // Act
        var result = await _controller.UpdateCampSite(campSiteId, updateRequest);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        var expectedMessage = $"{updateRequest.Name} har blivit uppdaterad";
        Assert.AreEqual(expectedMessage, okResult.Value);
    }

    [TestMethod]
    public async  Task UpdateCampSite_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 666;
        _mockCampSite
            .Setup(s => s.GetByIdAsync(invalidId))
            .ReturnsAsync((CampSite)null);
        var request = new UpdateCampSiteRequest
        {
            Id = invalidId,
            Name = "Updated Horse Camp",
            Description = "Updated description",
            Adress = "Updated Address"
        };

        // Act
        var result = await _controller.UpdateCampSite(invalidId, request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        var notFoundResult = (NotFoundObjectResult)result;
        var expectedMessage = $"{invalidId} Finns ej, you silly goose";
        Assert.AreEqual(expectedMessage, notFoundResult.Value);
    }
    [TestMethod]
    public async Task UpdateCampSite_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var campSiteId = 1;
        var request = new UpdateCampSiteRequest
        {
            Id = campSiteId,
            Name = "",
            Description = "Updated description",
            Adress = "Updated Address"
        };
        _controller.ModelState.AddModelError("Name", "Name is required");
        // Act
        var result = await _controller.UpdateCampSite(campSiteId, request);
        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestResult));
    }
    [TestMethod]
    public async Task UpdateCampSite_IdMisMatch_ReturnsBadRequest()
    {
        // Arrange
        var campSiteId = 1;
        var request = new UpdateCampSiteRequest
        {
            Id = 3, 
            Name = "Updated Horse Camp",
            Description = "Updated description",
            Adress = "Updated Address"
        };
        // Act
        var result = await _controller.UpdateCampSite(campSiteId, request);
        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestResult));
    }

    // ------------------------------------------ UpdateAsync Tests ------------------------------------------

    // ------------------------------------------ DeleteAsync Tests ------------------------------------------

    [TestMethod]
    public async Task DeleteCampSite_ValidId_ReturnsVeriNice()
    {
        // Arrange
        var campSiteId = 1;
        var campToDelete = new CampSite
        {
            Id = campSiteId,
            Name = "Horser Camp",
            Description = "A horse spot",
            Adress = "123 Camp St"
        };
        _mockCampSite
            .Setup(s => s.GetByIdAsync(campSiteId))
            .ReturnsAsync(campToDelete);

        _mockCampSite.
            Setup(s => s.DeleteAsync(campSiteId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteCampSite(campSiteId);

        // Assert 
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        var expectedMessage = $"{campToDelete.Name} har nu tagits bort";
        Assert.AreEqual(expectedMessage, okResult.Value);
    }

    [TestMethod]
    public async Task DeleteCampSite_NonExistingId_ReturnsNotFoundAndSillyText()
    {
        // Arrange
        var NonExistingId = 666;
        _mockCampSite
            .Setup(s => s.GetByIdAsync(NonExistingId))
            .ReturnsAsync((CampSite)null);

        // Act
        var resultat = await _controller.DeleteCampSite(NonExistingId);

        // Assert
        Assert.IsInstanceOfType(resultat, typeof(NotFoundObjectResult));
        var notFoundResult = (NotFoundObjectResult)resultat;
        var expectedMessage = $"Finns ingen campingplats med id: {NonExistingId}";
        Assert.AreEqual(expectedMessage, notFoundResult.Value);
    }

    [TestMethod]
    public async Task DeleteCampSite_CampSiteNotFound_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 666;
        _mockCampSite
            .Setup(s => s.GetByIdAsync(invalidId))
            .ReturnsAsync((CampSite?)null);

        // Act
        var result = await _controller.DeleteCampSite(invalidId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        var notFoundResult = (NotFoundObjectResult)result;
        var expectedMessage = $"Finns ingen campingplats med id: {invalidId}";
        Assert.AreEqual(expectedMessage, notFoundResult.Value);
    }

    [TestMethod]
    public async Task DeleteCampSite_ServiceThrowsException_PropagatesException()
    {
        // Arrange
        var campSiteId = 1;
        var existingCampSite = new CampSite { Id = campSiteId, Name = "Test Camp" };

        _mockCampSite
            .Setup(s => s.GetByIdAsync(campSiteId))
            .ReturnsAsync(existingCampSite);

        _mockCampSite
            .Setup(s => s.DeleteAsync(campSiteId))
            .ThrowsAsync(new Exception("Service error"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(async () => await _controller.DeleteCampSite(campSiteId));
    }

    // ------------------------------------------ DeleteAsync Tests ------------------------------------------
}
