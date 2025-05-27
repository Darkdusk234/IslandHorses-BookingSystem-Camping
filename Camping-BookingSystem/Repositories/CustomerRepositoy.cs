using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace Camping_BookingSystem.Repositories;

public class CustomerRepositoy : ICustomerRepository
{
    private readonly CampingDbContext _context;

    public CustomerRepositoy(CampingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync(); 
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id); 
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
    }

    public void Update(Customer customer)
    {
        _context.Customers.Update(customer);
    }

    public void Delete(Customer customer)
    {
        _context.Customers.Remove(customer);
    }
    
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    // Custom method for validating customer by email for new bookings with customer details.
    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
    }
}