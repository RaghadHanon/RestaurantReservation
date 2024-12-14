using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Db.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public ReservationRepository(RestaurantReservationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<bool> ReservationExistsAsync(int reservationId)
    {
        return await _dbContext.Reservations.AnyAsync(r => r.ReservationId == reservationId);
    }

    public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
    {
        return await _dbContext.Reservations.ToListAsync();
    }

    public async Task<Reservation?> GetReservationAsync(int reservationId, bool includeOrders = false)
    {
        if (includeOrders)
        {
            return await _dbContext.Reservations
                .Include(r => r.Orders)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        }
        return await _dbContext.Reservations
                     .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByCustomerIdAsync(int customerID)
    {
        return await _dbContext.Reservations
            .Where(r => r.CustomerId == customerID)
            .ToListAsync();
    }
    public Reservation CreateReservation(Reservation reservation)
    {
        _dbContext.Reservations.Add(reservation);
        return reservation;
    }

    public void DeleteReservation(Reservation reservation)
    {
        _dbContext.Reservations.Remove(reservation);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _dbContext.SaveChangesAsync() >= 0);
    }
}