using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Repositories;

public interface ICustomerRepository
{
    //Default methods for CRUD operations
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsynch(Customer customer);
}