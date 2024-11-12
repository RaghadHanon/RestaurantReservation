using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;
public class OrderItemRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public OrderItemRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderItem?> AddOrderItemAsync(OrderItem orderItem)
    {
        if (orderItem == null)
            return null;

        await _dbContext.OrderItems.AddAsync(orderItem);
        await _dbContext.SaveChangesAsync();
        return orderItem;
    }

    public async Task<bool> RemoveOrderItemAsync(int orderItemId)
    {
        if (orderItemId == 0)
            return false;

        var orderItem = await GetOrderItemByIdAsync(orderItemId);
        if (orderItem == null)
            return false;

        _dbContext.OrderItems.Remove(orderItem);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateOrderItemAsync(int orderItemId, int? quantity = null, int? menuItemId = null)
    {
        if (orderItemId == 0)
            return false;

        var orderItem = await GetOrderItemByIdAsync(orderItemId);
        if (orderItem == null)
            return false;

        if (quantity.HasValue)
            orderItem.Quantity = quantity.Value;

        if (menuItemId.HasValue)
        {
            orderItem.ItemId = menuItemId.Value;
            orderItem.MenuItem = await _dbContext.MenuItems.FindAsync(menuItemId.Value);
        }

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<OrderItem>> GetAllOrderItemsAsync()
    {
        return await _dbContext.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.MenuItem)
            .ToListAsync();
    }

    public async Task<OrderItem?> GetOrderItemByIdAsync(int orderItemId)
    {
        if (orderItemId == 0)
            return null;

        return await _dbContext.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.MenuItem)
            .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);
    }

    public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
    {
        return await _dbContext.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .Include(oi => oi.Order)
            .Include(oi => oi.MenuItem)
            .ToListAsync();
    }

    public async Task<List<OrderItem>> GetOrderItemsByMenuItemIdAsync(int menuItemId)
    {
        return await _dbContext.OrderItems
            .Where(oi => oi.ItemId == menuItemId)
            .Include(oi => oi.Order)
            .Include(oi => oi.MenuItem)
            .ToListAsync();
    }
}
