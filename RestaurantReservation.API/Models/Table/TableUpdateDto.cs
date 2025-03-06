namespace RestaurantReservation.API.ModelView.Table;

/// <summary>
/// Data transfer object representing the capacity information for updating a table.
/// </summary>
public class TableUpdateDto
{
    /// <summary>
    /// The capacity of the table.
    /// </summary>
    public int Capacity { get; set; }
}