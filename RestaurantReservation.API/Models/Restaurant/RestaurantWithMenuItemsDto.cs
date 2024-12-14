using RestaurantReservation.API.Models.MenuItem;

namespace RestaurantReservation.API.ModelView.Restaurant;

public class RestaurantWithMenuItemsDto
{
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string OpeningHours { get; set; }
    public List<MenuItemDto> MenuItems { get; set; }
}