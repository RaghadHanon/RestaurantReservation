using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Db.Models;
public class MenuItem
{
    [Key]
    public int MenuItemId { get; set; }
    [Required]
    public int RestaurantId { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(250)]
    public string Description { get; set; }
    [Required]
    public decimal Price { get; set; }

    public Restaurant Restaurant { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}
