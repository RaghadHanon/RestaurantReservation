using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Db.Models;
public class Customer
{
    [Key]
    public int CustomerId { get; set; }
    [Required, MaxLength(50)]
    public string FirstName { get; set; }
    [Required, MaxLength(50)]
    public string LastName { get; set; }
    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; }
    [Phone]
    public string PhoneNumber { get; set; }

    public List<Reservation> Reservations { get; set; }
}

