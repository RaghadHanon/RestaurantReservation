using RestaurantReservation.Db.Entities;
using System.Text.Json.Serialization;

namespace RestaurantReservation.API.Models.MenuItem;
public class MenuItemDto
{
    public int ItemId { get; set; }
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}
