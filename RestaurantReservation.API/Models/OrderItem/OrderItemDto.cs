using RestaurantReservation.API.Models.MenuItem;

namespace RestaurantReservation.API.Models.OrderItem;
public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public MenuItemDto MenuItem { get; set; }
}
