namespace RestaurantReservation.API.Models.OrderItem;

/// <summary>
/// Data transfer object for creating an order item.
/// </summary>
public class OrderItemCreationDto
{
    /// <summary>
    /// The ID of the menu item associated with the order item.
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// The quantity of the menu item in the order.
    /// </summary>
    public int Quantity { get; set; }
}