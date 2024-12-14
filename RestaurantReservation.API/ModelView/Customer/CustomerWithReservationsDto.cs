namespace RestaurantReservation.API.Models.Customer;
public class CustomerWithReservationsDto
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int NumberOfReservations
    {
        get
        {
            return Reservations.Count;
        }
    }

    public List<Db.Entities.Reservation> Reservations { get; set; } = new List<Db.Entities.Reservation>();
}

