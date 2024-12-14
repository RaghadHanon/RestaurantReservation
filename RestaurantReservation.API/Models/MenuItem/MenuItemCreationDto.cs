namespace RestaurantReservation.API.Models.MenuItem;

/// <summary>
/// Data transfer object for creating a new menu item.
/// </summary>
public class MenuItemCreationDto
{
    /// <summary>
    /// Name of the menu item.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the menu item.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Price of the menu item.
    /// </summary>
    public decimal Price { get; set; }
}