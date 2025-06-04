using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Repositories;


namespace Camping_BookingSystem.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    { 
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id); 
    }

    public async Task<(bool Success, string? ErrorMessage)> CreateCustomerAsync(Customer customer)
    {
        var validation = await ValidateCustomerAsync(customer);
        if (!validation.IsValid)
        {
            return (false, validation.ErrorMessage);
        }
    
        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateCustomerAsync(Customer customer)
    {
        var validation = await ValidateCustomerAsync(customer);
        if (!validation.IsValid)
        {
            return (false, validation.ErrorMessage);
        }
    
        _customerRepository.Update(customer);
        await _customerRepository.SaveAsync();
        return (true, null);
    }

    public async Task DeleteCustomerAsync(Customer customer)
    {
        _customerRepository.Delete(customer);
        await _customerRepository.SaveAsync(); 
    }

    public async Task<(bool IsValid, string? ErrorMessage)> ValidateCustomerAsync(Customer customer)
    {
        var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(customer.Email);
        if (existingCustomer != null && existingCustomer.Id != customer.Id)
        {
            return (false, "Email is already in use by another customer.");
        }

        return (true, null);
    }
}