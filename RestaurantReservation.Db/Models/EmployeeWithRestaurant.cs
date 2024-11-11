using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Db.Models;
public record EmployeeWithRestaurant
{
    [Required, MaxLength(200)]
    public string RestaurantName { get; set; }
    [Required, MaxLength(200)]
    public string RestaurantAddress { get; set; }
    [Required, Phone]
    public string RestaurantPhoneNumber { get; set; }
    [Required, MaxLength(50)]
    public string OpeningHours { get; set; }
    [Required, MaxLength(110)]
    public string EmployeeName { get; set; }
    [Required, MaxLength(50)]
    public string Position { get; set; }
}
