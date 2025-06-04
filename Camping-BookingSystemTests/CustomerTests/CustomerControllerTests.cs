using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CustomerDTOs;
using Camping_BookingSystem.Controllers;
using Camping_BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Camping_BookingSystemTests.ControllerTests;

[TestClass]
public class CustomerControllerTests
{
    private Mock<ICustomerService> _customerServiceMock;
    private CustomerController _controller;

    [TestInitialize]
    public void Setup()
    {
        _customerServiceMock = new Mock<ICustomerService>();
        _controller = new CustomerController(_customerServiceMock.Object);
    }

    [TestMethod]
    public async Task GetAllCustomers_GivenCustomersExist_WhenCalled_ThenReturnsOkWithCustomerResponses()
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
            }
        };
        _customerServiceMock.Setup(s => s.GetAllCustomersAsync()).ReturnsAsync(customers);

        // When
        var result = await _controller.GetAllCustomers();

        // Then
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var data = okResult.Value as IEnumerable<CustomerResponse>;
        Assert.AreEqual(1, data.Count());
    }

    [TestMethod]
    public async Task GetCustomerById_GivenCustomerExists_WhenCalled_ThenReturnsOkWithCustomer()
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
        _customerServiceMock.Setup(s => s.GetCustomerByIdAsync(1)).ReturnsAsync(customer);

        // When
        var result = await _controller.GetCustomerById(1);

        // Then
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as CustomerResponse;
        Assert.AreEqual("Anna", response.FirstName);
    }

    [TestMethod]
    public async Task CreateCustomer_GivenValidDto_WhenCalled_ThenReturnsCreatedAtAction()
    {
        // Given: a dto object and a customer
        var dto = new CreateCustomerDto
        {
            FirstName = "Sven",
            LastName = "Svensson",
            Email = "sven@svenssons.com",
            PhoneNumber = "0732333333",
            StreetAddress = "Ohmgatan 3",
            ZipCode = "67892",
            City = "Östersund"
        };
        
        var createdCustomer = new Customer
        {
            Id = 99,
            FirstName = "Sven",
            LastName = "Svensson",
            Email = "sven@svenssons.com",
            PhoneNumber = "0732333333",
            StreetAddress = "Ohmgatan 3",
            ZipCode = "67892",
            City = "Östersund"
        };

        _customerServiceMock
            .Setup(s => s.CreateCustomerAsync(It.IsAny<Customer>()))
            .ReturnsAsync(createdCustomer);

        // When
        var result = await _controller.CreateCustomer(dto);

        // Then
        var createdResult = result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);

        var response = createdResult.Value as CustomerResponse;
        Assert.IsNotNull(response);
        Assert.AreEqual(99, response.Id);
        Assert.AreEqual("Sven", response.FirstName);
        Assert.AreEqual("Svensson", response.LastName);
        Assert.AreEqual("sven@svenssons.com", response.Email);
        Assert.AreEqual("0732333333", response.PhoneNumber);
        Assert.AreEqual("Ohmgatan 3", response.StreetAddress);
        Assert.AreEqual("67892", response.ZipCode);
        Assert.AreEqual("Östersund", response.City);
    }

    [TestMethod]
    public async Task DeleteCustomer_GivenCustomerExists_WhenCalled_ThenReturnsNoContent()
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
        _customerServiceMock.Setup(s => s.GetCustomerByIdAsync(5)).ReturnsAsync(customer);

        // When
        var result = await _controller.DeleteCustomer(5);

        // Then
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        _customerServiceMock.Verify(s => s.DeleteCustomerAsync(customer), Times.Once);
    }
}