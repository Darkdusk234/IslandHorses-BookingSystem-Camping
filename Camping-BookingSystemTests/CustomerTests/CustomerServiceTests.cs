using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Services;
using Camping_BookingSystem.Repositories;
using Moq;

namespace Camping_BookingSystemTests.CustomerTests;

[TestClass]
public class CustomerServiceTests
{
    private Mock<ICustomerRepository> _customerRepoMock;
    private CustomerService _customerService;

    [TestInitialize]
    public void Setup()
    {
        _customerRepoMock = new Mock<ICustomerRepository>();
        _customerService = new CustomerService(_customerRepoMock.Object);
    }

    [TestMethod]
    public async Task GetAllCustomersAsync_GivenCustomersExist_WhenCalled_ThenReturnsAllCustomers()
    {
        // Given
        var customers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                FirstName = "Anna",
                LastName = "Andersson",
                Email = "anna@example.com",
                PhoneNumber = "0701234567",
                StreetAddress = "Storgatan 1",
                ZipCode = "12345",
                City = "Stockholm"
                
            },
            new Customer
            {
                Id = 2,
                FirstName = "Sven",
                LastName = "Svensson",
                Email = "sven@svenssons.com",
                PhoneNumber = "0730236667",
                StreetAddress = "Ohmgatan 3",
                ZipCode = "67892",
                City = "Östersund"
            }
        };
        _customerRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

        // When
        var result = await _customerService.GetAllCustomersAsync();

        // Then
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public async Task GetCustomerByIdAsync_GivenCustomerExists_WhenCalled_ThenReturnsCustomer()
    {
        // Given
        var customer = new Customer
        {
            Id = 1,
            FirstName = "Anna",
            LastName = "Andersson",
            Email = "anna@example.com",
            PhoneNumber = "0701234567",
            StreetAddress = "Storgatan 1",
            ZipCode = "12345",
            City = "Stockholm"
        };
        _customerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);

        // When
        var result = await _customerService.GetCustomerByIdAsync(1);

        // Then
        Assert.IsNotNull(result);
        Assert.AreEqual("Anna", result.FirstName);
    }

    [TestMethod]
    public async Task CreateCustomerAsync_GivenValidCustomer_WhenCalled_ThenAddsAndSavesCustomer()
    {
        // Given
        var customer = new Customer
        {
            FirstName = "Olle",
            LastName = "Olau",
            Email = "olle@odla.com",
            PhoneNumber = "0730111222",
            StreetAddress = "Stolpgatan 6",
            ZipCode = "13579",
            City = "Borlänge"
        };

        _customerRepoMock.Setup(r => r.AddAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);
        _customerRepoMock.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

        // When
        var (success, errorMessage) = await _customerService.CreateCustomerAsync(customer);

        // Then
        _customerRepoMock.Verify(r => r.AddAsync(It.Is<Customer>(c => 
            c.FirstName == "Olle")), Times.Once);
        _customerRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        
        Assert.IsTrue(success);
        Assert.IsNull(errorMessage);
    }

    [TestMethod]
    public async Task UpdateCustomerAsync_GivenValidCustomer_WhenCalled_ThenUpdatesAndSavesCustomer()
    {
        // Given
        var customer = new Customer
        {
            Id = 2,
            FirstName = "Sven",
            LastName = "Svensson",
            Email = "sven@svenssons.com",
            PhoneNumber = "0732333333",
            StreetAddress = "Ohmgatan 3",
            ZipCode = "67892",
            City = "Östersund"
        };

        // When
        await _customerService.UpdateCustomerAsync(customer);

        // Then
        // Verifies that Update() is called once and once only with this customer object. If not, an error is thrown.
        _customerRepoMock.Verify(r => r.Update(customer), Times.Once);
        // Verifies that SaveAsync() is called once and once. If not, an error is thrown.
        _customerRepoMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [TestMethod]
    public async Task DeleteCustomerAsync_GivenValidCustomer_WhenCalled_ThenDeletesAndSavesCustomer()
    {
        // Given
        var customer = new Customer
        {
            Id = 5,
            FirstName = "Lars",
            LastName = "Larsson",
            Email = "lars@larsson.com",
            PhoneNumber = "0740444444",
            StreetAddress = "Stolpgatan 4",
            ZipCode = "444444",
            City = "Lillgrund"
        };

        // When
        await _customerService.DeleteCustomerAsync(customer);

        // Then
        // Verifies that Delete() is called once and once only with this customre object. If not, an error is thrown.
        _customerRepoMock.Verify(r => r.Delete(customer), Times.Once);
        // Verifies that SaveAsync() is called once and once. If not, an error is thrown.
        _customerRepoMock.Verify(r => r.SaveAsync(), Times.Once);   
    }
}