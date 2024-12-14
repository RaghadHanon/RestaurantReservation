using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantReservation.Db.Entities;
public class MenuItem
{
    [Key]
    public int ItemId { get; set; }
    [JsonIgnore]
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    [JsonIgnore]
    public Restaurant Restaurant { get; set; }
    [JsonIgnore]
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
