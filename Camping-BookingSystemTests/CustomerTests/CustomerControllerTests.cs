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
            new Customer { Id = 1, FirstName = "Anna" }
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
        var customer = new Customer { Id = 1, FirstName = "Anna" };
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
        // Given
        var dto = new CreateCustomerDto { FirstName = "Lisa" };
        var createdCustomer = new Customer { Id = 99, FirstName = "Lisa" };
        _customerServiceMock.Setup(s => s.CreateCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(createdCustomer);

        // When
        var result = await _controller.CreateCustomer(dto);

        // Then
        var createdResult = result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);
        var response = createdResult.Value as CustomerResponse;
        Assert.AreEqual("Lisa", response.FirstName);
        Assert.AreEqual(99, response.Id);
    }

    [TestMethod]
    public async Task DeleteCustomer_GivenCustomerExists_WhenCalled_ThenReturnsNoContent()
    {
        // Given
        var customer = new Customer { Id = 5 };
        _customerServiceMock.Setup(s => s.GetCustomerByIdAsync(5)).ReturnsAsync(customer);

        // When
        var result = await _controller.DeleteCustomer(5);

        // Then
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        _customerServiceMock.Verify(s => s.DeleteCustomerAsync(customer), Times.Once);
    }
}