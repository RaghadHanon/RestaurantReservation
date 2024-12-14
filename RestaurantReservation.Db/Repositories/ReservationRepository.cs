using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        var query = _dbContext.Reservations.AsQueryable();
        if (includeOrders)
        {
            query = query.Include(r => r.Orders);
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
    public async Task<Reservation> CreateReservationAsync(Reservation reservation)
    {
        await _dbContext.Reservations.AddAsync(reservation);
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