using BookingSystem_ClassLibrary.Models;
using BookingSystem_ClassLibrary.Models.DTOs.BookingDTOs;
using BookingSystem_ClassLibrary.Models.DTOs.CustomerDTOs;

namespace Camping_BookingSystem.Mapping;

//Borrow mapper structure from BookingMapper(by Fredrik) for a more consistent approach. 
public static class CustomerMapper
{
    public static CustomerResponse ToCustomerResponse(this Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            StreetAddress = customer.StreetAddress,
            ZipCode = customer.ZipCode,
            City = customer.City,
            Bookings = customer.Bookings?.Select(b => b.ToBookingDetailsResponse()).ToList()
        };
    }
    
    public static Customer ToCustomer(this CreateCustomerDto dto)
    {
        return new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            StreetAddress = dto.StreetAddress,
            ZipCode = dto.ZipCode,
            City = dto.City
        };
    }
}