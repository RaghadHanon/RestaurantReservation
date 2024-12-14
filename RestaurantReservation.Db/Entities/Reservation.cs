using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantReservation.Db.Entities;
public class Reservation
{
    [Key]
    public int ReservationId { get; set; }
    public int CustomerId { get; set; }
    public int RestaurantId { get; set; }
    public int TableId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int PartySize { get; set; }

    [JsonIgnore]
    public Restaurant Restaurant { get; set; }
    [JsonIgnore]
    public Table Table { get; set; }
    [JsonIgnore]
    public Customer Customer { get; set; }
    [JsonIgnore]
    public List<Order> Orders { get; set; } = new List<Order>();
}

