using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Services;
using Camping_BookingSystem.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            new Customer { Id = 1, FirstName = "Anna" },
            new Customer { Id = 2, FirstName = "Olle" }
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
        var customer = new Customer { Id = 1, FirstName = "Anna" };
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
        var customer = new Customer { FirstName = "Lisa" };

        // When
        var result = await _customerService.CreateCustomerAsync(customer);

        // Then
        _customerRepoMock.Verify(r => r.AddAsync(customer), Times.Once);
        _customerRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        Assert.AreEqual("Lisa", result.FirstName);
    }

    [TestMethod]
    public async Task UpdateCustomerAsync_GivenValidCustomer_WhenCalled_ThenUpdatesAndSavesCustomer()
    {
        // Given
        var customer = new Customer { Id = 5, FirstName = "Mats" };

        // When
        await _customerService.UpdateCustomerAsync(customer);

        // Then
        _customerRepoMock.Verify(r => r.Update(customer), Times.Once);
        _customerRepoMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [TestMethod]
    public async Task DeleteCustomerAsync_GivenValidCustomer_WhenCalled_ThenDeletesAndSavesCustomer()
    {
        // Given
        var customer = new Customer { Id = 6, FirstName = "Erik" };

        // When
        await _customerService.DeleteCustomerAsync(customer);

        // Then
        _customerRepoMock.Verify(r => r.Delete(customer), Times.Once);
        _customerRepoMock.Verify(r => r.SaveAsync(), Times.Once);
    }
}