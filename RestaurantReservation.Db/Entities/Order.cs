namespace RestaurantReservation.Db.Entities;
public class Order
{
    public int OrderId { get; set; }
    public int ReservationId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public Employee Employee { get; set; }
    public Reservation Reservation { get; set; }
    public List<OrderItem> OrderItems { get; set; } 
}

