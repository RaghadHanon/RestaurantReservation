namespace RestaurantReservation.API.Models.MenuItem;

/// <summary>
/// Data transfer object for updating an existing menu item.
/// </summary>
public class MenuItemUpdateDto
{
    /// <summary>
    /// The ID of the restaurant to which the menu item belongs.
    /// </summary>
    public int RestaurantId { get; set; }

    /// <summary>
    /// Updated name of the menu item.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Updated description of the menu item.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Updated price of the menu item.
    /// </summary>
    public decimal Price { get; set; }
}