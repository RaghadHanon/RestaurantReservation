using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Db.Models;
public class Employee
{
    [Key]
    public int EmployeeId { get; set; }
    [Required]
    public int RestaurantId { get; set; }
    [Required, MaxLength(50)]
    public string FirstName { get; set; }
    [Required, MaxLength(50)]
    public string LastName { get; set; }
    [Required, MaxLength(50)]
    public string Position { get; set; }

    public Restaurant Restaurant { get; set; }
    public List<Order> Orders { get; set; }

      
}

