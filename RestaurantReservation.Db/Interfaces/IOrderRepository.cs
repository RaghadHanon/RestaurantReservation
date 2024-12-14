using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Interfaces
{
    public interface IOrderRepository
    {
        Task<decimal> CalculateTotalAmount(int orderId);
        void CreateOrder(int reservationId, Order order);
        void DeleteOrder(Order order);
        Task<Order?> GetOrderAsync(int reservationId, int orderId);
        Task<IEnumerable<Order>> GetOrdersForReservationAsync(int reservationId);
        Task<bool> OrderExistsAsync(int reservationId, int orderId);
        Task<bool> SaveChangesAsync();
    }
}