using RestaurantReservation.API.Models.Order;
using RestaurantReservation.API.ModelView.Restaurant;

namespace RestaurantReservation.API.Models.Employee;
public class EmployeeWithOrdersDto
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }
    public string RestaurantName { get; set; }
    public List<OrderWithoutOrderItemsDto> Orders { get; set; }
}
