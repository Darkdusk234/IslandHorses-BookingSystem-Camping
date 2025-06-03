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
        
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                errorMessage = "Validation failed.",
                details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }
        
        var newCustomer = dto.ToCustomer(); 
        var createdCustomer = await _customerService.CreateCustomerAsync(newCustomer);
        var response = createdCustomer.ToCustomerResponse(); 
        
        return CreatedAtAction(nameof(GetCustomerById), new { id = response.Id }, response);
    }

    [HttpPut("{id}", Name = "UpdateCustomer")]
    public async Task<IActionResult> UpdateCustomer([FromRoute] int id, [FromBody] UpdateCustomerDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                errorMessage = "Validation failed.",
                details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (id <= 0)
        {
            return BadRequest(new { errorMessage = "Invalid customer ID" });
        }

        var customerToUpdate = await _customerService.GetCustomerByIdAsync(id);

        if (customerToUpdate == null)
        {
            return NotFound(new { errorMessage = "Customer not found!" });
        }

        customerToUpdate.FirstName = dto.FirstName;
        customerToUpdate.LastName = dto.LastName;
        customerToUpdate.Email = dto.Email;
        customerToUpdate.PhoneNumber = dto.PhoneNumber;
        customerToUpdate.StreetAddress = dto.StreetAddress;
        customerToUpdate.ZipCode = dto.ZipCode;
        customerToUpdate.City = dto.City;

        await _customerService.UpdateCustomerAsync(customerToUpdate);
        return NoContent();
    }

    [HttpDelete(Name = "DeleteCustomer")]
    public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { erroMessage = "Invalid customer ID" });
        }
        var customerToDelete = await _customerService.GetCustomerByIdAsync(id); 

        if (customerToDelete == null)
        {
            return NotFound(new { errorMessage = "Customer not found!" });
        }
        await _customerService.DeleteCustomerAsync(customerToDelete);

        return NoContent(); 
    }
}