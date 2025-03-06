using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Interfaces;
public interface IReservationRepository
{
    Task<Reservation> CreateReservationAsync(Reservation reservation);
    void DeleteReservation(Reservation reservation);
    Task<IEnumerable<Reservation>> GetAllReservationsAsync();
    Task<Reservation?> GetReservationAsync(int reservationId, bool includeOrders = false);
    Task<IEnumerable<Reservation>> GetReservationsByCustomerIdAsync(int customerID);
    Task<bool> ReservationExistsAsync(int reservationId);
    Task<bool> SaveChangesAsync();
}