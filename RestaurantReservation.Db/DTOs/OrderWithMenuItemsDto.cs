namespace RestaurantReservation.Db.DTOs;
public class OrderWithMenuItemsDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<MenuItemDto> MenuItems { get; set; }
}