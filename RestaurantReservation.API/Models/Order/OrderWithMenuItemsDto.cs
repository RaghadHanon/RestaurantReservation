using RestaurantReservation.API.Models.MenuItem;

namespace RestaurantReservation.Db.DTOs;
public record OrderWithMenuItemsDto
{
    public int OrderId { get; set; }
    public int ReservationId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<MenuItemDto> MenuItems { get; set; }
}