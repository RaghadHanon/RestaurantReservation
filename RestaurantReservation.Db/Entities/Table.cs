using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantReservation.Db.Entities;
public class Table
{
    [Key]
    public int TableId { get; set; }
    public int RestaurantId { get; set; }
    public int Capacity { get; set; }

    [JsonIgnore]
    public Restaurant Restaurant { get; set; }
    [JsonIgnore]
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
}

