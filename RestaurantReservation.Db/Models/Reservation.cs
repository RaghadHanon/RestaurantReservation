using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Db.Models;
public class Reservation
{
    [Key]
    public int ReservationId { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public int RestaurantId { get; set; }
    [Required]
    public int TableId { get; set; }
    [Required]
    public DateTime ReservationDate { get; set; }
    [Required]
    public int PartySize { get; set; }

    public Restaurant Restaurant { get; set; }
    public Table Table { get; set; }
    public Customer Customer { get; set; }
    public List<Order> Orders { get; set; }
}

