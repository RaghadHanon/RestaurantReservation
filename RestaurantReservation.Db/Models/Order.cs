using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Db.Models;
public class Order
{
    [Key]
    public int OrderId { get; set; }
    [Required]
    public int ReservationId { get; set; }
    [Required]
    public int EmployeeId { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    public decimal TotalAmount { get; set; }

    public Employee Employee { get; set; }
    public Reservation Reservation { get; set; }    
    public List<OrderItem> OrderItems { get; set; }  
}

