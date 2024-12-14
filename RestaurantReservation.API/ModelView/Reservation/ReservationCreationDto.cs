namespace RestaurantReservation.API.Models.Reservation;

/// <summary>
/// Data transfer object for creating a reservation.
/// </summary>
public class ReservationCreationDto
{
    /// <summary>
    /// The ID of the customer making the reservation.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// The ID of the restaurant where the reservation is made.
    /// </summary>
    public int RestaurantId { get; set; }

    /// <summary>
    /// The ID of the table reserved for the party.
    /// </summary>
    public int TableId { get; set; }

    /// <summary>
    /// The date and time of the reservation.
    /// </summary>
    public DateTime ReservationDate { get; set; }

    /// <summary>
    /// The size of the party for the reservation.
    /// </summary>
    public int PartySize { get; set; }
}