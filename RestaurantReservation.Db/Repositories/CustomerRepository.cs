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
        var query = _dbContext.Customers.AsQueryable();
        if (includeReservations)
        {
            query = query.Include(c => c.Reservations);
        }

        return await query.FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        await _dbContext.Customers.AddAsync(customer);
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