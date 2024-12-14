using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Models.Employee;
public class EmployeeWithOrdersDto
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }
    public Restaurant Restaurant { get; set; }
    public List<Db.Entities.Order> Orders { get; set; }
}
