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

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveAsync();
        return customer; 
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        _customerRepository.Update(customer);
        await _customerRepository.SaveAsync(); 
    }

    public async Task DeleteCustomerAsynch(Customer customer)
    {
        _customerRepository.Delete(customer);
        await _customerRepository.SaveAsync(); 
    }
}