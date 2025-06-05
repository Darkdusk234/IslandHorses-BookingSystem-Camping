using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Services;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task<(bool Success, string? ErrorMessage)> CreateCustomerAsync(Customer customer);
    public Task<(bool Success, string? ErrorMessage)> UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(Customer customer);
}