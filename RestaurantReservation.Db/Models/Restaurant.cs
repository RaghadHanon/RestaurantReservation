using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Db.Models;
public class Restaurant
{
    [Key]
    public int RestaurantId { get; set; }
    [Required, MaxLength(200)]
    public string Name { get; set; }
    [Required, MaxLength(200)]
    public string Address { get; set; }
    [Phone]
    public string PhoneNumber { get; set; }
    [MaxLength(50)]
    public string OpeningHours { get; set; }

    public List<Table> Tables { get; set; }
    public List<Reservation> Reservations { get; set; }
    public List<Employee> Employees { get; set; }
    public List<MenuItem> MenuItems { get; set; }
}
