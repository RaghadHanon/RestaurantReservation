using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;
public class CustomerRepository
{
    private RestaurantReservationDbContext _dbContext;
    public CustomerRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Customer?> AddCustomerAsync(Customer customer)
    {
        if (customer == null)
            return null;

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> RemoveCustomerAsync(int customerId)
    {
        if (customerId == 0)
            return false;

        var customer = await GetCustomerByIdAsync(customerId);
        if (customer == null)
            return false;

        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateCustomerAsync(int customerId, string? firstName = null, string? lastName = null, string? email = null, string? phoneNumber = null)
    {
        if (customerId == 0)
            return false;

        var customer = await GetCustomerByIdAsync(customerId);
        if (customer == null)
            return false;

        if (!firstName.IsNullOrEmpty())
            customer.FirstName = firstName;

        if (!lastName.IsNullOrEmpty())
            customer.LastName = lastName;

        if (!email.IsNullOrEmpty())
            customer.Email = email;

        if (!phoneNumber.IsNullOrEmpty())
            customer.PhoneNumber = phoneNumber;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        return await _dbContext.Customers
                               .Include(c => c.Reservations)
                               .ToListAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        if (customerId == 0)
            return null;

        return await _dbContext.Customers
                               .Include(c => c.Reservations)
                               .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }
}
