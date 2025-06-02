using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Data;
using Camping_BookingSystem.Services;
using Camping_BookingSystem.Repositories;

namespace Camping_BookingSystemTests
{
    [TestClass]
    public class SpotTypeServiceTests
    {
        private Mock<ISpotTypeRepository> _mockRepository;
        private SpotTypeService _service;


        [TestInitialize]    // This method runs before each test method
        public void Setup()         // a setup method to initialize the mock repository and service
        {
            _mockRepository = new Mock<ISpotTypeRepository>();
            _service = new SpotTypeService(_mockRepository.Object);
        }

        // This method runs after each test method 
        [TestCleanup]
        public void Cleanup()       // a cleanup method to reset the state after each test
        {
            _mockRepository = null;
            _service = null;
        }

        // ------------------------------------------ GetAllAsync Tests ------------------------------------------

        [TestMethod]
        public async Task GetAllAsync_GivenExistingSpotTypes_WhenCalled_ThenShouldReturnAllSpotTypes()
        {
            // Given - We have existing SpotTypes in the system
            var givenExistingSpotTypes = new List<SpotType>
            {
                new SpotType { Id = 1, Name = "Tent", Price = 150.00m, MaxPersonLimit = 4 },
                new SpotType { Id = 2, Name = "Mobile Home", Price = 300.50m, MaxPersonLimit = 6 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(givenExistingSpotTypes);

            // When - We call GetAllSpotTypesAsync method
            var whenResult = await _service.GetAllSpotTypesAsync();

            // Then - We expect to get all SpotTypes back
            Assert.IsNotNull(whenResult);
            Assert.AreEqual(2, whenResult.Count());

            var resultList = whenResult.ToList();
            Assert.AreEqual("Tent", resultList[0].Name);
            Assert.AreEqual(150.00m, resultList[0].Price);
            Assert.AreEqual("Mobile Home", resultList[1].Name);
            Assert.AreEqual(300.50m, resultList[1].Price);

            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllAsync_GivenNoSpotTypesExist_WhenCalled_ThenShouldReturnEmptyCollection()
        {
            // Given - No SpotTypes exist in the system
            var givenEmptySpotTypeList = new List<SpotType>();

            _mockRepository.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(givenEmptySpotTypeList);

            // When - We call GetAllSpotTypesAsync method
            var whenResult = await _service.GetAllSpotTypesAsync();

            // Then - We expect an empty collection
            Assert.IsNotNull(whenResult);
            Assert.AreEqual(0, whenResult.Count());
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllAsync_GivenRepositoryThrowsException_WhenCalled_ThenShouldPropagateException()
        {
            // Given - Repository throws an exception
            var givenExpectedException = new InvalidOperationException("Database is unavailable");

            _mockRepository.Setup(repo => repo.GetAllAsync())
                          .ThrowsAsync(givenExpectedException);

            // When & Then - We expect the exception to propagate
            var whenThenException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _service.GetAllSpotTypesAsync());

            Assert.AreEqual("Database is unavailable", whenThenException.Message);
        }

        // ------------------------------------------ GetAllAsync Tests ------------------------------------------

        // ------------------------------------------ GetByIdAsync Tests ------------------------------------------

        [TestMethod]
        public async Task GetByIdAsync_GivenValidExistingId_WhenCalled_ThenShouldReturnCorrectSpotType()
        {
            // Given - We have a valid SpotType with ID 1
            var givenSpotTypeId = 1;
            var givenExpectedSpotType = new SpotType
            {
                Id = givenSpotTypeId,
                Name = "Premium Tent",
                Price = 250.75m,
                MaxPersonLimit = 8
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(givenSpotTypeId))
                          .ReturnsAsync(givenExpectedSpotType);

            // When - We call GetSpotTypeByIdAsync with valid ID
            var whenResult = await _service.GetSpotTypeByIdAsync(givenSpotTypeId);

            // Then - We expect the correct SpotType back
            Assert.IsNotNull(whenResult);
            Assert.AreEqual(givenExpectedSpotType.Id, whenResult.Id);
            Assert.AreEqual(givenExpectedSpotType.Name, whenResult.Name);
            Assert.AreEqual(givenExpectedSpotType.Price, whenResult.Price);
            Assert.AreEqual(givenExpectedSpotType.MaxPersonLimit, whenResult.MaxPersonLimit);
            _mockRepository.Verify(repo => repo.GetByIdAsync(givenSpotTypeId), Times.Once);
        }

        [TestMethod]
        public async Task GetByIdAsync_GivenNonExistentId_WhenCalled_ThenShouldReturnNull()
        {
            // Given - We have a non-existent ID
            var givenNonExistentId = 666;

            _mockRepository.Setup(repo => repo.GetByIdAsync(givenNonExistentId))
                          .ReturnsAsync((SpotType?)null);

            // When - We call GetSpotTypeByIdAsync with non-existent ID
            var whenResult = await _service.GetSpotTypeByIdAsync(givenNonExistentId);

            // Then - We expect null
            Assert.IsNull(whenResult);
            _mockRepository.Verify(repo => repo.GetByIdAsync(givenNonExistentId), Times.Once);
        }

        [TestMethod]
        public async Task GetByIdAsync_GivenZeroId_WhenCalled_ThenShouldReturnNull()
        {
            // Given - We have an invalid ID (0)
            var givenInvalidId = 0;

            _mockRepository.Setup(repo => repo.GetByIdAsync(givenInvalidId))
                          .ReturnsAsync((SpotType?)null);

            // When - We call GetSpotTypeByIdAsync with invalid ID
            var whenResult = await _service.GetSpotTypeByIdAsync(givenInvalidId);

            // Then - We expect null
            Assert.IsNull(whenResult);
            _mockRepository.Verify(repo => repo.GetByIdAsync(givenInvalidId), Times.Once);
        }

        [TestMethod]
        public async Task GetByIdAsync_GivenNegativeId_WhenCalled_ThenShouldReturnNull()
        {
            // Given - We have a negative ID
            var givenNegativeId = -5;

            _mockRepository.Setup(repo => repo.GetByIdAsync(givenNegativeId))
                          .ReturnsAsync((SpotType?)null);

            // When - We call GetSpotTypeByIdAsync with negative ID
            var whenResult = await _service.GetSpotTypeByIdAsync(givenNegativeId);

            // Then - We expect null
            Assert.IsNull(whenResult);
            _mockRepository.Verify(repo => repo.GetByIdAsync(givenNegativeId), Times.Once);
        }

        // ------------------------------------------ GetByIdAsync Tests ------------------------------------------

        // ------------------------------------------ CreateAsync Tests ------------------------------------------

        [TestMethod]
        public async Task CreateAsync_GivenValidSpotType_WhenCalled_ThenShouldReturnCreatedSpotType()
        {
            // Given - We have a valid new SpotType
            var givenNewSpotType = new SpotType
            {
                Name = "Luxury RV",
                Price = 500.00m,
                MaxPersonLimit = 4
            };

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<SpotType>()))
                          .Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.SaveAsync())
                          .Returns(Task.CompletedTask);

            // When - We call CreateSpotTypeAsync with the new SpotType
            var whenResult = await _service.CreateSpotTypeAsync(givenNewSpotType);

            // Then - We expect to get back the created SpotType
            Assert.IsNotNull(whenResult);
            Assert.AreEqual(givenNewSpotType.Name, whenResult.Name);
            Assert.AreEqual(givenNewSpotType.Price, whenResult.Price);
            Assert.AreEqual(givenNewSpotType.MaxPersonLimit, whenResult.MaxPersonLimit);
            _mockRepository.Verify(repo => repo.AddAsync(givenNewSpotType), Times.Once);
            _mockRepository.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task CreateAsync_GivenNullSpotType_WhenCalled_ThenShouldThrowArgumentNullException()
        {
            // Given - We have a null SpotType
            SpotType? givenNullSpotType = null;

            // When & Then - We expect ArgumentNullException
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => _service.CreateSpotTypeAsync(givenNullSpotType!));

            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<SpotType>()), Times.Never);
        }

        [TestMethod]
        public async Task CreateAsync_GivenSpotTypeWithEmptyName_WhenCalled_ThenShouldThrowArgumentException()
        {
            // Given - We have a SpotType with empty name
            var givenInvalidSpotType = new SpotType
            {
                Name = "",
                Price = 200.00m,
                MaxPersonLimit = 2
            };

            // When & Then - We expect ArgumentException
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _service.CreateSpotTypeAsync(givenInvalidSpotType));

            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<SpotType>()), Times.Never);
        }

        [TestMethod]
        public async Task CreateAsync_GivenSpotTypeWithNegativePrice_WhenCalled_ThenShouldThrowArgumentException()
        {
            // Given - We have a SpotType with negative price
            var givenInvalidSpotType = new SpotType
            {
                Name = "Cheap Tent",
                Price = -50.00m,
                MaxPersonLimit = 2
            };

            // When & Then - We expect ArgumentException
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _service.CreateSpotTypeAsync(givenInvalidSpotType));

            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<SpotType>()), Times.Never);
        }

        [TestMethod]
        public async Task CreateAsync_GivenSpotTypeWithZeroMaxPersonLimit_WhenCalled_ThenShouldThrowArgumentException()
        {
            // Given - We have a SpotType with 0 max persons
            var givenInvalidSpotType = new SpotType
            {
                Name = "Empty Tent",
                Price = 100.00m,
                MaxPersonLimit = 0
            };

            // When & Then - We expect ArgumentException
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _service.CreateSpotTypeAsync(givenInvalidSpotType));

            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<SpotType>()), Times.Never);
        }

        // ------------------------------------------ CreateAsync Tests ------------------------------------------

        // ------------------------------------------ UpdateAsync Tests ------------------------------------------

        [TestMethod]
        public async Task UpdateAsync_GivenValidSpotType_WhenCalled_ThenShouldUpdateSuccessfully()
        {
            // Given - We have a valid SpotType to update
            var givenSpotTypeToUpdate = new SpotType
            {
                Id = 1,
                Name = "Updated Tent",
                Price = 175.50m,
                MaxPersonLimit = 6
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SpotType>()))
                          .Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.SaveAsync())
                          .Returns(Task.CompletedTask);

            // When - We call UpdateSpotTypeAsync
            await _service.UpdateSpotTypeAsync(givenSpotTypeToUpdate);

            // Then - We expect the update to be called
            _mockRepository.Verify(repo => repo.UpdateAsync(givenSpotTypeToUpdate), Times.Once);
            _mockRepository.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_GivenNullSpotType_WhenCalled_ThenShouldThrowArgumentNullException()
        {
            // Given - We have a null SpotType
            SpotType? givenNullSpotType = null;

            // When & Then - We expect ArgumentNullException
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => _service.UpdateSpotTypeAsync(givenNullSpotType!));

            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<SpotType>()), Times.Never);
        }

        // ------------------------------------------ UpdateAsync Tests ------------------------------------------

        // ------------------------------------------ DeleteAsync Tests ------------------------------------------

        [TestMethod]
        public async Task DeleteAsync_GivenValidId_WhenCalled_ThenShouldDeleteSuccessfully()
        {
            // Given - We have a valid ID to delete
            var givenValidId = 1;

            _mockRepository.Setup(repo => repo.GetByIdAsync(givenValidId))
                          .ReturnsAsync(new SpotType { Id = givenValidId });
            _mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<SpotType>()))
                          .Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.SaveAsync())
                          .Returns(Task.CompletedTask);

            // When - We call DeleteSpotTypeAsync
            await _service.DeleteSpotTypeAsync(givenValidId);

            // Then - We expect the delete operations to be called
            _mockRepository.Verify(repo => repo.GetByIdAsync(givenValidId), Times.Once);
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SpotType>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_GivenNonExistentId_WhenCalled_ThenShouldNotCallDelete()
        {
            // Given - We have a non-existent ID
            var givenNonExistentId = 666;

            _mockRepository.Setup(repo => repo.GetByIdAsync(givenNonExistentId))
                          .ReturnsAsync((SpotType?)null);

            // When - We call DeleteSpotTypeAsync with non-existent ID
            await _service.DeleteSpotTypeAsync(givenNonExistentId);

            // Then - Repository is called but no deletion occurs
            _mockRepository.Verify(repo => repo.GetByIdAsync(givenNonExistentId), Times.Once);
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SpotType>()), Times.Never);
            _mockRepository.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAsync_GivenRepositoryThrowsException_WhenCalled_ThenShouldPropagateException()
        {
            // Given - Repository throws an exception during deletion
            var givenValidId = 1;
            var givenExpectedException = new InvalidOperationException("Cannot delete SpotType that is in use");

            _mockRepository.Setup(repo => repo.GetByIdAsync(givenValidId))
                          .ThrowsAsync(givenExpectedException);

            // When & Then - We expect the exception to propagate
            var whenThenException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _service.DeleteSpotTypeAsync(givenValidId));

            Assert.AreEqual("Cannot delete SpotType that is in use", whenThenException.Message);
        }

        // ------------------------------------------ DeleteAsync Tests ------------------------------------------
    }
}
