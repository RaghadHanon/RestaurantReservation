using RestaurantReservation.Db.Entities;
using System.Text.Json.Serialization;

namespace RestaurantReservation.API.Models.Employee;
public class EmployeeDto
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }
    public Restaurant Restaurant { get; set; }
}
