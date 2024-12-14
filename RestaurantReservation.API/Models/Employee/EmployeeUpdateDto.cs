namespace RestaurantReservation.API.Models.Employee;

/// <summary>
/// Data transfer object for Updating an employee.
/// </summary>
public class EmployeeUpdateDto
{

    /// <summary>
    /// ID of the restaurant to which the employee belongs.
    /// </summary>
    public int RestaurantId { get; set; }

    /// <summary>
    /// First name of the employee.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Last name of the employee.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Position or role of the employee.
    /// </summary>
    public string Position { get; set; }
}
