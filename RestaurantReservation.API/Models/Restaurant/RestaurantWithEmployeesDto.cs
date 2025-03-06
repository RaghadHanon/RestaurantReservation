using RestaurantReservation.API.Models.Employee;

namespace RestaurantReservation.API.ModelView.Restaurant;

public class RestaurantWithEmployeesDto
{
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string OpeningHours { get; set; }
    public List<EmployeeDto> Employees { get; set; }
}