using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantReservation.Db.Entities;
public class Employee
{
    [Key]
    public int EmployeeId { get; set; }
    [JsonIgnore]
    public int RestaurantId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }

    [JsonIgnore]
    public Restaurant Restaurant { get; set; }
    [JsonIgnore]
    public List<Order> Orders { get; set; } = new List<Order>();
}

