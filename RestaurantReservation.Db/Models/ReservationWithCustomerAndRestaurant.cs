using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Db.Models;
public record ReservationWithCustomerAndRestaurant
{
    public int ReservationId { get; set; }
    [Required]
    public DateTime ReservationDate { get; set; }
    [Required]
    public int PartySize { get; set; }
    [Required, MaxLength(200)]
    public string RestaurantName { get; set; }
    [Required, MaxLength(200)]
    public string RestaurantAddress { get; set; }
    [Required, Phone]
    public string RestaurantPhoneNumber { get; set; }
    [Required, MaxLength(50)]
    public string OpeningHours { get; set; }
    [Required, MaxLength(110)]
    public string CustomerName { get; set; }
    [Required, EmailAddress, MaxLength(100)]
    public string CustomerEmail { get; set; }
    [Required, Phone]
    public string CustomerPhoneNumber { get; set; }
}
