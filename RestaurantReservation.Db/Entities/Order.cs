using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantReservation.Db.Entities;
public class Order
{
    [Key]
    public int OrderId { get; set; }
    public int ReservationId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }

    [JsonIgnore]
    public Employee Employee { get; set; }
    [JsonIgnore]
    public Reservation Reservation { get; set; }
    [JsonIgnore]
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

