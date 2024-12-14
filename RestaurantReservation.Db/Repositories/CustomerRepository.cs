using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Db.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public CustomerRepository(RestaurantReservationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<bool> CustomerExistsAsync(int id)
    {
        return await _dbContext.Customers.AnyAsync(c => c.CustomerId == id);
    }
    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _dbContext.Customers.ToListAsync();
    }

    public async Task<Customer?> GetCustomerAsync(int id, bool includeReservations = false)
    {
        if (includeReservations)
        {
            return await _dbContext.Customers.Include(c => c.Reservations).FirstOrDefaultAsync(c => c.CustomerId == id);
        }
        return await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public Customer CreateCustomer(Customer customer)
    {
        _dbContext.Customers.Add(customer);
        return customer;
    }

    public Customer UpdateCustomer(Customer customer)
    {
        _dbContext.Customers.Update(customer);
        return customer;
    }

    public void DeleteCustomer(Customer customer)
    {
        _dbContext.Customers.Remove(customer);
    }

    public List<Customer> FindCustomersWithPartySizeGreaterThan(int partySize)
    {
        return _dbContext.Customers
            .FromSqlRaw("EXEC FindCustomersByPartySize @PartySize", partySize).ToList();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _dbContext.SaveChangesAsync() >= 0);
    }
}