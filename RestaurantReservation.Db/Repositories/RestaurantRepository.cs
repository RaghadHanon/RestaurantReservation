using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class RestaurantRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public RestaurantRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Restaurant?> AddRestaurantAsync(Restaurant restaurant)
    {
        if (restaurant == null)
            return null;

        await _dbContext.Restaurants.AddAsync(restaurant);
        await _dbContext.SaveChangesAsync();
        return restaurant;
    }

    public async Task<bool> RemoveRestaurantAsync(int restaurantId)
    {
        if (restaurantId == 0)
            return false;

        var restaurant = await GetRestaurantByIdAsync(restaurantId);
        if (restaurant == null)
            return false;

        _dbContext.Restaurants.Remove(restaurant);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateRestaurantAsync(int restaurantId, string? name = null, string? address = null, string? phoneNumber = null, string? openingHours = null)
    {
        if (restaurantId == 0)
            return false;

        var restaurant = await GetRestaurantByIdAsync(restaurantId);
        if (restaurant == null)
            return false;

        if (!string.IsNullOrEmpty(name))
            restaurant.Name = name;

        if (!string.IsNullOrEmpty(address))
            restaurant.Address = address;

        if (!string.IsNullOrEmpty(phoneNumber))
            restaurant.PhoneNumber = phoneNumber;

        if (!string.IsNullOrEmpty(openingHours))
            restaurant.OpeningHours = openingHours;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Restaurant>> GetAllRestaurantsAsync()
    {
        return await _dbContext.Restaurants
            .Include(r => r.Tables)
            .Include(r => r.Reservations)
            .Include(r => r.Employees)
            .Include(r => r.MenuItems)
            .ToListAsync();
    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(int restaurantId)
    {
        if (restaurantId == 0)
            return null;

        return await _dbContext.Restaurants
            .Include(r => r.Tables)
            .Include(r => r.Reservations)
            .Include(r => r.Employees)
            .Include(r => r.MenuItems)
            .FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);
    }

    public async Task<List<Table>> GetTablesByRestaurantIdAsync(int restaurantId)
    {
        return await _dbContext.Tables
            .Where(t => t.RestaurantId == restaurantId)
            .ToListAsync();
    }

    public async Task<List<MenuItem>> GetMenuItemsByRestaurantIdAsync(int restaurantId)
    {
        return await _dbContext.MenuItems
            .Where(mi => mi.RestaurantId == restaurantId)
            .ToListAsync();
    }

    public async Task<List<Employee>> GetEmployeesByRestaurantIdAsync(int restaurantId)
    {
        return await _dbContext.Employees
            .Where(e => e.RestaurantId == restaurantId)
            .ToListAsync();
    }

    public async Task<List<Reservation>> GetReservationsByRestaurantIdAsync(int restaurantId)
    {
        return await _dbContext.Reservations
            .Where(r => r.RestaurantId == restaurantId)
            .Include(r => r.Customer)
            .Include(r => r.Table)
            .Include(r => r.Orders)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalRevenueAsync(int restaurantId)
    {
        return await _dbContext.Restaurants
            .Where(r => r.RestaurantId == restaurantId)
            .Select(r => _dbContext.GetTotalRevenue(r.RestaurantId))
            .SingleAsync();
    }

    public async Task<List<Customer>> FindCustomersByPartySizeAsync(int partySize)
    {
        return await _dbContext.Customers
            .FromSqlInterpolated($"EXEC FindCustomersByPartySize @PartySize = {partySize}")
            .ToListAsync();
    }
}
