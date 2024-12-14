using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.API.Models.Customer;

/// <summary>
/// Data transfer object for creating a customer.
/// </summary>
public class CustomerCreationDto 
{
    /// <summary>
    /// The first name of the customer.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// The last name of the customer.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the customer.
    /// </summary>
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The Phone number of the customer.
    /// </summary>
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
}