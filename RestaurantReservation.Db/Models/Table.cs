using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Db.Models;
public class Table
{
    [Key]
    public int TableId { get; set; }
    [Required]
    public int RestaurantId { get; set; }
    [Required]
    public int Capacity { get; set; }

    public Restaurant Restaurant { get; set; }
    public List<Reservation> Reservations { get; set; }
}

