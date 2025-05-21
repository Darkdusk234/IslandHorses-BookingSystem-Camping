using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Repositories;
using Camping_BookingSystem.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Camping_BookingSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository; 
    }

    [HttpGet(Name = "GetallCustomers")]
    public async Task<ActionResult<ICollection<Customer>>> GetAllCustomers()
    {
        return Ok(await _customerRepository.GetAllAsync()); 
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public async Task<ActionResult<Customer>> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            return NotFound(new { errorMessage = "Customer not found!" });
        }

        return Ok(customer); 
    }

}