using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantReservation.Db.Entities;
public class Restaurant
{
    [Key]
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    [Phone]
    public string PhoneNumber { get; set; }
    public string OpeningHours { get; set; }

    [JsonIgnore]
    public List<Table> Tables { get; set; } = new List<Table>();
    [JsonIgnore]
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    [JsonIgnore]
    public List<Employee> Employees { get; set; } = new List<Employee>();
    [JsonIgnore]
    public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
