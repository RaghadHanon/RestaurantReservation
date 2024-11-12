using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Db.Models;
public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }
    [Required]
    public int OrderId { get; set; }
    [Required]
    public int ItemId { get; set; }
    [Required]
    public int Quantity { get; set; }

    public Order Order { get; set; }
    public MenuItem MenuItem { get; set; }
}

