using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantReservation.Db.Entities;
public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }

    [JsonIgnore]
    public Order Order { get; set; }
    [JsonIgnore]
    public MenuItem MenuItem { get; set; }
}

