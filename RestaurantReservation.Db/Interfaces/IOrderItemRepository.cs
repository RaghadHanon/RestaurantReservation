using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Interfaces;
public interface IOrderItemRepository
{
    Task AddOrderItemToOrderAsync(int orderId, OrderItem orderItem);
    void DeleteOrderItem(OrderItem orderItem);
    Task<OrderItem?> GetOrderItemAsync(int reservationId, int orderId, int orderItemId);
    Task<IEnumerable<OrderItem>> GetOrderItemsForOrderAsync(int reservationId, int orderId);
    Task<bool> SaveChangesAsync(int orderId);
}
