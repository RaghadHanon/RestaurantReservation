using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Db.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public MenuItemRepository(RestaurantReservationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<bool> MenuItemExistsAsync(int id)
    {
        return await _dbContext.MenuItems.AnyAsync(m => m.ItemId == id);
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsInRestaurantAsync(int restaurantId)
    {
        return await _dbContext.MenuItems.Where(m => m.RestaurantId == restaurantId).ToListAsync();
    }

    public async Task<MenuItem?> GetMenuItemAsync(int restaurantId, int menuItemId)
    {
        return await _dbContext.MenuItems
            .FirstOrDefaultAsync(m => m.RestaurantId == restaurantId
                                   && m.ItemId == menuItemId);
    }

    public async Task<IEnumerable<MenuItem>> GetOrderedMenuItemsByReservationIdAsync(int reservationId)
    {
        return await _dbContext.OrderItems
            .Where(oi => oi.Order.ReservationId == reservationId)
            .Select(oi => oi.MenuItem)
            .ToListAsync();
    }

    public MenuItem CreateMenuItem(int restaurantId, MenuItem menuItem)
    {
        _dbContext.MenuItems.Add(menuItem);
        menuItem.RestaurantId = restaurantId;

        return menuItem;
    }

    public void DeleteMenuItem(MenuItem menuItem)
    {
        _dbContext.MenuItems.Remove(menuItem);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _dbContext.SaveChangesAsync() >= 0);
    }
}