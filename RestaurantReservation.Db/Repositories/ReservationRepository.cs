using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.DTOs;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;
public class ReservationRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public ReservationRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Reservation?> AddReservationAsync(Reservation reservation)
    {
        if (reservation == null)
            return null;

        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();
        return reservation;
    }

    public async Task<bool> RemoveReservationAsync(int reservationId)
    {
        if (reservationId == 0)
            return false;

        var reservation = await GetReservationByIdAsync(reservationId);
        if (reservation == null)
            return false;

        _dbContext.Reservations.Remove(reservation);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateReservationAsync(int reservationId, DateTime? reservationDate = null, int? partySize = null, int? tableId = null)
    {
        if (reservationId == 0)
            return false;

        var reservation = await GetReservationByIdAsync(reservationId);
        if (reservation == null)
            return false;

        if (reservationDate.HasValue)
            reservation.ReservationDate = reservationDate.Value;

        if (partySize.HasValue)
            reservation.PartySize = partySize.Value;

        if (tableId.HasValue)
        {
            reservation.TableId = tableId.Value;
            reservation.Table = await _dbContext.Tables.FindAsync(tableId.Value);
        }

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        return await _dbContext.Reservations
            .Include(r => r.Restaurant)
            .Include(r => r.Table)
            .Include(r => r.Customer)
            .Include(r => r.Orders)
            .ToListAsync();
    }

    public async Task<Reservation?> GetReservationByIdAsync(int reservationId)
    {
        if (reservationId == 0)
            return null;

        return await _dbContext.Reservations
            .Include(r => r.Restaurant)
            .Include(r => r.Table)
            .Include(r => r.Customer)
            .Include(r => r.Orders)
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
    }

    public async Task<List<Reservation>> GetReservationsByCustomerIdAsync(int customerId)
    {
        return await _dbContext.Reservations
            .Where(r => r.CustomerId == customerId)
            .Include(r => r.Restaurant)
            .Include(r => r.Table)
            .Include(r => r.Orders)
            .ToListAsync();
    }

    public async Task<List<Reservation>> GetReservationsByRestaurantIdAsync(int restaurantId)
    {
        return await _dbContext.Reservations
            .Where(r => r.RestaurantId == restaurantId)
            .Include(r => r.Customer)
            .Include(r => r.Table)
            .Include(r => r.Orders)
            .ToListAsync();
    }

    public async Task<List<Reservation>> GetReservationsByDateAsync(DateTime reservationDate)
    {
        return await _dbContext.Reservations
            .Where(r => r.ReservationDate.Date == reservationDate.Date)
            .Include(r => r.Restaurant)
            .Include(r => r.Table)
            .Include(r => r.Customer)
            .Include(r => r.Orders)
            .ToListAsync();
    }

    public async Task<List<ReservationWithCustomerAndRestaurantDto>> GetAllReservationWithCustomerAndRestaurantAsync()
    {
        return await _dbContext.ReservationWithCustomerAndRestaurant.ToListAsync();
    }
}
