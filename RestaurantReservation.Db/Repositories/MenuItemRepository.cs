using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;
public class MenuItemRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public MenuItemRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MenuItem?> AddMenuItemAsync(MenuItem menuItem)
    {
        if (menuItem == null)
            return null;

        await _dbContext.MenuItems.AddAsync(menuItem);
        await _dbContext.SaveChangesAsync();
        return menuItem;
    }

    public async Task<bool> RemoveMenuItemAsync(int menuItemId)
    {
        if (menuItemId == 0)
            return false;

        var menuItem = await GetMenuItemByIdAsync(menuItemId);
        if (menuItem == null)
            return false;

        _dbContext.MenuItems.Remove(menuItem);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateMenuItemAsync(int menuItemId, string? name = null, string? description = null, decimal? price = null)
    {
        if (menuItemId == 0)
            return false;

        var menuItem = await GetMenuItemByIdAsync(menuItemId);
        if (menuItem == null)
            return false;

        if (!string.IsNullOrEmpty(name))
            menuItem.Name = name;

        if (!string.IsNullOrEmpty(description))
            menuItem.Description = description;

        if (price.HasValue)
            menuItem.Price = price.Value;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<MenuItem>> GetAllMenuItemsAsync()
    {
        return await _dbContext.MenuItems
            .Include(m => m.Restaurant)
            .Include(m => m.OrderItems)
            .ToListAsync();
    }

    public async Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId)
    {
        if (menuItemId == 0)
            return null;

        return await _dbContext.MenuItems
            .Include(m => m.Restaurant)
            .Include(m => m.OrderItems)
            .FirstOrDefaultAsync(m => m.ItemId == menuItemId);
    }

    public async Task<List<MenuItem>> GetMenuItemsByRestaurantAsync(int restaurantId)
    {
        return await _dbContext.MenuItems
            .Where(m => m.RestaurantId == restaurantId)
            .Include(m => m.Restaurant)
            .Include(m => m.OrderItems)
            .ToListAsync();
    }

}
