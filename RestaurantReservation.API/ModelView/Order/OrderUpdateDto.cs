namespace RestaurantReservation.API.Models.Order;

/// <summary>
/// Data transfer object for updating an existing order.
/// </summary>
public class OrderUpdateDto
{
    /// <summary>
    /// The ID of the reservation associated with the order.
    /// </summary>
    public int ReservationId { get; set; }

    /// <summary>
    /// The ID of the employee placing the order.
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    /// The date of the order.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// The Total Amount of the order
    /// </summary>
    public decimal TotalAmount { get; set; }
}