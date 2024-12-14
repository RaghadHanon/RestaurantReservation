namespace RestaurantReservation.API.ModelView.Restaurant;

/// <summary>
/// Data transfer object for creating a resturant.
/// </summary>
public class RestaurantCreationDto
{
    /// <summary>
    /// The name of the restaurant.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The address of the restaurant.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// The phone number of the restaurant.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// The opening hours of the restaurant.
    /// </summary>
    public string OpenningHours { get; set; }
}