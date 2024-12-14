namespace RestaurantReservation.API.ModelView.Table;

/// <summary>
/// Data transfer object representing the capacity information for creating a table.
/// </summary>
public class TableCreationDto
{
    /// <summary>
    /// The capacity of the table.
    /// </summary>
    public int Capacity { get; set; }
}