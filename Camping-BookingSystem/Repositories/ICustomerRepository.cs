using BookingSystem_ClassLibrary.Models;

namespace Camping_BookingSystem.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task AddAsync(Customer customer);
    void Update(Customer customer);
    void Delete(Customer customer);
    public Task SaveAsync();
}