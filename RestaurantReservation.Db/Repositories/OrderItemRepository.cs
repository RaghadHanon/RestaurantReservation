using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Db.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly RestaurantReservationDbContext _dbContext;
    private readonly IOrderRepository _orderRepository;

    public OrderItemRepository(RestaurantReservationDbContext context,
        IOrderRepository orderRepository)
    {
        _dbContext = context;
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsForOrderAsync(int reservationId, int orderId)
    {
        return await _dbContext.OrderItems
            .Include(oi => oi.MenuItem)
            .Where(oi => oi.Order.ReservationId == reservationId && oi.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<OrderItem?> GetOrderItemAsync(int reservationId, int orderId, int orderItemId)
    {
        return await _dbContext.OrderItems
                         .Include(oi => oi.MenuItem)
                         .FirstOrDefaultAsync(oi =>
                                              oi.Order.ReservationId == reservationId &&
                                              oi.OrderId == orderId &&
                                              oi.OrderItemId == orderItemId);
    }

    public void AddOrderItemToOrder(int orderId, OrderItem orderItem)
    {
        orderItem.OrderId = orderId;
        _dbContext.OrderItems.Add(orderItem);
    }

    public void DeleteOrderItem(OrderItem orderItem)
    {
        _dbContext.OrderItems.Remove(orderItem);
    }

    public async Task<bool> SaveChangesAsync(int orderId)
    {
        await _dbContext.SaveChangesAsync();
        var order = _dbContext.Orders.Find(orderId);
        if (order != null)
        {
            order.TotalAmount = await _orderRepository.CalculateTotalAmount(orderId);
        }
        return (await _dbContext.SaveChangesAsync() >= 0);
    }
}