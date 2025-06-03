using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Services;
using Moq;

namespace Camping_BookingSystemTests;

[TestClass]
public class CampSiteServiceTests
{
    private Mock<ICampSiteRepository> _mockRepository;
    private CampSiteService _service;

    [TestInitialize]
    
    // This method runs before each test method
    public void Setup()     // a setup method to initialize the mock repository and service
    {
        _mockRepository = new Mock<ICampSiteRepository>();
        _service = new CampSiteService(_mockRepository.Object);
    }

    // This method runs after each test method 
    [TestCleanup]
    public void Cleanup()   // a cleanup method to reset the state after each test
    {
        _mockRepository = null;
        _service = null;
    }

    // ------------------------------------------ GetAllAsync Tests ------------------------------------------

    [TestMethod]
    public async Task GetAllAsync_WithExistingCampSites_ShouldReturnAllCampSites()
    {
        // Arrange
        var expectedCampSites = new List<CampSite>
            {
                new CampSite { Id = 1, Name = "Häst Camping", Description = "Det är rimligt med häst camping", Adress = "Hästvägen 2" },
                new CampSite { Id = 2, Name = "Campus Campoing", Description = "Vid Varbergs kust", Adress = "Jämte fästningen" }
            };

        _mockRepository.Setup(repo => repo.GetAllCampSitesAsync())
                      .ReturnsAsync(expectedCampSites);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
        CollectionAssert.AreEqual(expectedCampSites.ToList(), result.ToList());
        _mockRepository.Verify(repo => repo.GetAllCampSitesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetAllAsync_WhenNoCampSitesExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAllCampSitesAsync())
                      .ReturnsAsync(new List<CampSite>());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
        _mockRepository.Verify(repo => repo.GetAllCampSitesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetAllAsync_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Database connection failed");
        _mockRepository.Setup(repo => repo.GetAllCampSitesAsync())
                      .ThrowsAsync(expectedException);

        // Act & Assert
        var thrownException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            () => _service.GetAllAsync());

        Assert.AreEqual("Database connection failed", thrownException.Message);
    }

    // ------------------------------------------ GetAllAsync Tests ------------------------------------------

    // ------------------------------------------ GetByIdAsync Tests ------------------------------------------

    [TestMethod]
    public async Task GetByIdAsync_WithValidId_ShouldReturnCampSite()
    {
        // Arrange
        var expectedCampSite = new CampSite
        {
            Id = 1,
            Name = "Rimlig Häst Camping",
            Description = "Hästagård",
            Adress = "Hästgårds vägen 1"
        };

        _mockRepository.Setup(repo => repo.GetCampSiteByIdAsync(1))
                      .ReturnsAsync(expectedCampSite);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedCampSite.Id, result.Id);
        Assert.AreEqual(expectedCampSite.Name, result.Name);
        Assert.AreEqual(expectedCampSite.Description, result.Description);
        Assert.AreEqual(expectedCampSite.Adress, result.Adress);
        _mockRepository.Verify(repo => repo.GetCampSiteByIdAsync(1), Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetCampSiteByIdAsync(666))
                      .ReturnsAsync((CampSite?)null);

        // Act
        var result = await _service.GetByIdAsync(666);

        // Assert
        Assert.IsNull(result);
        _mockRepository.Verify(repo => repo.GetCampSiteByIdAsync(666), Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_WithZeroId_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetCampSiteByIdAsync(0))
                      .ReturnsAsync((CampSite?)null);

        // Act
        var result = await _service.GetByIdAsync(0);

        // Assert
        Assert.IsNull(result);
        _mockRepository.Verify(repo => repo.GetCampSiteByIdAsync(0), Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_WithNegativeId_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetCampSiteByIdAsync(-1))
                      .ReturnsAsync((CampSite?)null);

        // Act
        var result = await _service.GetByIdAsync(-1);

        // Assert
        Assert.IsNull(result);
        _mockRepository.Verify(repo => repo.GetCampSiteByIdAsync(-1), Times.Once);
    }

    // ------------------------------------------ GetByIdAsync Tests ------------------------------------------

    // ------------------------------------------ CreateAsync Tests ------------------------------------------

    [TestMethod]
    public async Task CreateAsync_WithValidCampSite_ShouldReturnCreatedCampSite()
    {
        // Arrange
        var newCampSite = new CampSite
        {
            Name = "Campus Camping",
            Description = "Ett fint camping för elever",
            Adress = "JämteFästningenvägen 3"
        };

        _mockRepository.Setup(repo => repo.CreateCampSiteAsync(It.IsAny<CampSite>()))
                      .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(newCampSite);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(newCampSite.Name, result.Name);
        Assert.AreEqual(newCampSite.Description, result.Description);
        Assert.AreEqual(newCampSite.Adress, result.Adress);
        _mockRepository.Verify(repo => repo.CreateCampSiteAsync(newCampSite), Times.Once);
    }

    [TestMethod]
    public async Task CreateAsync_WithNullCampSite_ShouldThrowArgumentNullException()
    {
        // Arrange
        CampSite? nullCampSite = null;

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => _service.CreateAsync(nullCampSite!));

        _mockRepository.Verify(repo => repo.CreateCampSiteAsync(It.IsAny<CampSite>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateAsync_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidCampSite = new CampSite
        {
            Name = "",
            Description = "En lång beskrivning",
            Adress = "Addressväegn 1"
        };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(
            () => _service.CreateAsync(invalidCampSite));

        _mockRepository.Verify(repo => repo.CreateCampSiteAsync(It.IsAny<CampSite>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateAsync_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var validCampSite = new CampSite
        {
            Name = "Test Camp",
            Description = "Test Description",
            Adress = "Test Address"
        };

        var expectedException = new InvalidOperationException("Failed to save to database");
        _mockRepository.Setup(repo => repo.CreateCampSiteAsync(It.IsAny<CampSite>()))
                      .ThrowsAsync(expectedException);

        // Act & Assert
        var thrownException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            () => _service.CreateAsync(validCampSite));

        Assert.AreEqual("Failed to save to database", thrownException.Message);
    }

    // ------------------------------------------ CreateAsync Tests ------------------------------------------

    // ------------------------------------------ UpdateAsync Tests ------------------------------------------

    [TestMethod]
    public async Task UpdateAsync_WithValidCampSite_ShouldUpdateSuccessfully()
    {
        // Arrange
        var campSiteToUpdate = new CampSite
        {
            Id = 1,
            Name = "Bua Häst Camping",
            Description = "Någonstanns i Bua",
            Adress = "Bua"
        };

        _mockRepository.Setup(repo => repo.UpdateCampSiteAsync(It.IsAny<CampSite>()))
                      .ReturnsAsync(campSiteToUpdate);

        // Act
        await _service.UpdateAsync(campSiteToUpdate);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateCampSiteAsync(campSiteToUpdate), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_WithNullCampSite_ShouldThrowArgumentNullException()
    {
        // Arrange
        CampSite? nullCampSite = null;

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => _service.UpdateAsync(nullCampSite!));

        _mockRepository.Verify(repo => repo.UpdateCampSiteAsync(It.IsAny<CampSite>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateAsync_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var validCampSite = new CampSite
        {
            Id = 1,
            Name = "Test Name",
            Description = "Test Description",
            Adress = "Test Address 1"
        };

        var expectedException = new InvalidOperationException("Update failed");
        _mockRepository.Setup(repo => repo.UpdateCampSiteAsync(It.IsAny<CampSite>()))
                      .ThrowsAsync(expectedException);

        // Act & Assert
        var thrownException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            () => _service.UpdateAsync(validCampSite));

        Assert.AreEqual("Update failed", thrownException.Message);
    }

    // ------------------------------------------ UpdateAsync Tests ------------------------------------------

    // ------------------------------------------ DeleteAsync Tests ------------------------------------------

    [TestMethod]
    public async Task DeleteAsync_WithValidId_ShouldDeleteSuccessfully()
    {
        // Arrange
        var campSiteId = 1;
        _mockRepository.Setup(repo => repo.DeleteCampSiteAsync(campSiteId))
                      .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(campSiteId);

        // Assert
        _mockRepository.Verify(repo => repo.DeleteCampSiteAsync(campSiteId), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_WithInvalidId_ShouldStillCallRepository()
    {
        // Arrange
        var invalidId = 666;
        _mockRepository.Setup(repo => repo.DeleteCampSiteAsync(invalidId))
                      .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(invalidId);

        // Assert
        _mockRepository.Verify(repo => repo.DeleteCampSiteAsync(invalidId), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_WithZeroId_ShouldStillCallRepository()
    {
        // Arrange
        var zeroId = 0;
        _mockRepository.Setup(repo => repo.DeleteCampSiteAsync(zeroId))
                      .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(zeroId);

        // Assert
        _mockRepository.Verify(repo => repo.DeleteCampSiteAsync(zeroId), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var validId = 1;
        var expectedException = new InvalidOperationException("Delete failed");
        _mockRepository.Setup(repo => repo.DeleteCampSiteAsync(validId))
                      .ThrowsAsync(expectedException);

        // Act & Assert
        var thrownException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            () => _service.DeleteAsync(validId));

        Assert.AreEqual("Delete failed", thrownException.Message);
    }
    // ------------------------------------------ DeleteAsync Tests ------------------------------------------

}
