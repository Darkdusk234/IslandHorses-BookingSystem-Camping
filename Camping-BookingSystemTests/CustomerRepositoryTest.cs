using BookingSystem_ClassLibrary.Data;
using BookingSystem_ClassLibrary.Models;
using Camping_BookingSystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Camping_BookingSystemTests;

[TestClass]
public class CustomerRepositoryTest
{
    private CampingDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CampingDbContext>()
            .UseInMemoryDatabase($"BookingTestDb_{Guid.NewGuid()}")
            .Options;
        return new CampingDbContext(options);
    }

    [TestMethod]
    public async Task GetAllAsyncFetchAllCustomers()
    {
        // Given a in-memory database and added customers
        using var context = GetInMemoryDbContext();
        var repository = new CustomerRepositoy(context);

        var customer1 = new Customer
        {
            FirstName = "Alice",
            LastName = "Andersson",
            Email = "alice@example.com",
            PhoneNumber = "0701234567",
            StreetAddress = "Storgatan 1",
            ZipCode = "12345",
            City = "Göteborg"
        };

        var customer2 = new Customer
        {
            FirstName = "Bob",
            LastName = "Bergström",
            Email = "bob@example.com",
            PhoneNumber = "0707654321",
            StreetAddress = "Lillgatan 2",
            ZipCode = "54321",
            City = "Malmö"
        };

        context.AddRange(customer1, customer2);
        await context.SaveChangesAsync();
            
        //When fetching all customers from the in-memory database
        var fetchResult = await repository.GetAllAsync();

        //Then, both customers should be in fetchResult
        Assert.AreEqual(2, fetchResult.Count());
        Assert.IsTrue(fetchResult.Any(c => c.FirstName == "Alice" && c.LastName == "Andersson" ));
        Assert.IsTrue(fetchResult.Any(c => c.FirstName == "Bob" && c.LastName == "Bergström" ));
    }

    [TestMethod]
    public async Task GetByIdReturnCorrectCustomer()
    {
        // Given a in-memory database and added customers
        using var context = GetInMemoryDbContext();
        var repository = new CustomerRepositoy(context);
    
        var customer = new Customer
        {
            FirstName = "Bob",
            LastName = "Bergström",
            Email = "bob@example.com",
            PhoneNumber = "0707654321",
            StreetAddress = "Lillgatan 2",
            ZipCode = "54321",
            City = "Malmö"
        };

        context.Add(customer);
        await context.SaveChangesAsync();
    
        //When fetching a customer based on its ID, the correct customer should be returned
        var resultingCustmer = await repository.GetByIdAsync(customer.Id); 
        
        //Then
        Assert.IsNotNull(resultingCustmer);
        Assert.AreEqual(customer.FirstName, resultingCustmer.FirstName);
        Assert.AreEqual(customer.LastName, resultingCustmer.LastName);
    }
    
    [TestMethod]
    public async Task GetByIdReturnNullWhenIdDoesNotExist()
    {
        // Given a in-memory database and added customers
        using var context = GetInMemoryDbContext();
        var repository = new CustomerRepositoy(context);
    
        var customer = new Customer
        {
            FirstName = "Bob",
            LastName = "Bergström",
            Email = "bob@example.com",
            PhoneNumber = "0707654321",
            StreetAddress = "Lillgatan 2",
            ZipCode = "54321",
            City = "Malmö"
        };

        context.Add(customer);
        await context.SaveChangesAsync();
    
        //When fetching a customer that does not exist
        var resultingCustomer = await repository.GetByIdAsync(5); 
        
        //Then
        Assert.IsNull(resultingCustomer);
    }

    [TestMethod]
    public async Task AddAsynchAddCustomer()
    {
        // Given a in-memory database and a customer
        using var context = GetInMemoryDbContext();
        var repository = new CustomerRepositoy(context);
        
        var customer = new Customer
        {
            FirstName = "Bob",
            LastName = "Bergström",
            Email = "bob@example.com",
            PhoneNumber = "0707654321",
            StreetAddress = "Lillgatan 2",
            ZipCode = "54321",
            City = "Malmö"
        };

        //When adding the customer
        await repository.AddAsync(customer);
        
        //Then 
        var result = await repository.GetAllAsync(); 
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
    }
    
    [TestMethod]
    public async Task DeleteAsynchDeletesCustomer()
    {
        // Given a in-memory database and a added customer
        using var context = GetInMemoryDbContext();
        var repository = new CustomerRepositoy(context);
        
        var customer = new Customer
        {
            FirstName = "Bob",
            LastName = "Bergström",
            Email = "bob@example.com",
            PhoneNumber = "0707654321",
            StreetAddress = "Lillgatan 2",
            ZipCode = "54321",
            City = "Malmö"
        };
        
        context.Add(customer);
        await context.SaveChangesAsync();
        
        //When customer is deleted
        await repository.DeleteAsynch(customer); 
        
        //Then the list of customers chould be empty
        var result = await repository.GetAllAsync(); 
        Assert.AreEqual(0, result.Count());
    }
    
    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateCustomerFields()
    {
        //Given: 
        using var context = GetInMemoryDbContext();
        var repository = new CustomerRepositoy(context);

        var customer = new Customer
        {
            FirstName = "Alice",
            LastName = "Andersson",
            Email = "alice@example.com",
            PhoneNumber = "0701234567",
            StreetAddress = "Storgatan 1",
            ZipCode = "12345",
            City = "Göteborg"
        };

        context.Add(customer);
        await context.SaveChangesAsync();

        // When city is updated
        customer.City = "Stockholm";
        repository.Update(customer);
        
        // Clear ChangeTracker to simulate a new database connection
        context.ChangeTracker.Clear();
        
        //Then:
        var updated = await repository.GetByIdAsync(customer.Id);
        Assert.AreEqual("Stockholm", updated.City);
    }
}