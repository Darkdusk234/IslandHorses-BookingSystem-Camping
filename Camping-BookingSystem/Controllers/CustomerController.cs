using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.CustomerDTOs;
using Camping_BookingSystem.Mapping;
using Camping_BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace Camping_BookingSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
       _customerService = customerService; 
    }

    [HttpGet(Name = "GetallCustomers")]
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        var response = customers.Select(c => c.ToCustomerResponse());
        return Ok(response); 
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public async Task<ActionResult<Customer>> GetCustomerById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            return NotFound(new { errorMessage = "Customer not found!" });
        }

        var response = customer.ToCustomerResponse();
        return Ok(response); 
    }
    
    [HttpPost(Name = "CreateCustomer")] 
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        var newCustomer = dto.ToCustomer(); 
        var createdCustomer = await _customerService.CreateCustomerAsync(newCustomer);
        var response = createdCustomer.ToCustomerResponse(); 
        
        return CreatedAtAction(nameof(GetCustomerById), new { id = response.Id }, response);
    }

    [HttpPut(Name = "UpdateCustomer")]
    public async Task<IActionResult> UpdateCustomre(int id, [FromBody] UpdateCustomerDto updateCustomerDto)
    {
        var customerToUpdate = await _customerService.GetCustomerByIdAsync(id);

        if (customerToUpdate == null)
        {
            return NotFound(new { errorMessage = "Customer not found to update!" });
        }

        customerToUpdate.FirstName = updateCustomerDto.FirstName;
        customerToUpdate.LastName = updateCustomerDto.LastName;
        customerToUpdate.Email = updateCustomerDto.Email;
        customerToUpdate.PhoneNumber = updateCustomerDto.PhoneNumber;
        customerToUpdate.StreetAddress = updateCustomerDto.StreetAddress;
        customerToUpdate.ZipCode = updateCustomerDto.ZipCode;
        customerToUpdate.City = updateCustomerDto.City;

        await _customerService.UpdateCustomerAsync(customerToUpdate);
        return NoContent();
    }

    [HttpDelete(Name = "DeleteCustomer")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customerToDelete = await _customerService.GetCustomerByIdAsync(id); 

        if (customerToDelete == null)
        {
            return NotFound(new { errorMessage = "Kunden hittades inte." });
        }
        await _customerService.DeleteCustomerAsynch(customerToDelete);

        return NoContent(); 
    }
}