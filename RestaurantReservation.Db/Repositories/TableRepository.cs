using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class TableRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public TableRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Table?> AddTableAsync(Table table)
    {
        if (table == null)
            return null;

        await _dbContext.Tables.AddAsync(table);
        await _dbContext.SaveChangesAsync();
        return table;
    }

    public async Task<bool> RemoveTableAsync(int tableId)
    {
        if (tableId == 0)
            return false;

        var table = await GetTableByIdAsync(tableId);
        if (table == null)
            return false;

        _dbContext.Tables.Remove(table);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateTableAsync(int tableId, int? capacity = null)
    {
        if (tableId == 0)
            return false;

        var table = await GetTableByIdAsync(tableId);
        if (table == null)
            return false;

        if (capacity.HasValue)
            table.Capacity = capacity.Value;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Table>> GetAllTablesAsync()
    {
        return await _dbContext.Tables
            .Include(t => t.Restaurant)
            .Include(t => t.Reservations)
            .ToListAsync();
    }

    public async Task<Table?> GetTableByIdAsync(int tableId)
    {
        if (tableId == 0)
            return null;

        return await _dbContext.Tables
            .Include(t => t.Restaurant)
            .Include(t => t.Reservations)
            .FirstOrDefaultAsync(t => t.TableId == tableId);
    }

    public async Task<List<Reservation>> GetReservationsByTableIdAsync(int tableId)
    {
        return await _dbContext.Reservations
            .Where(r => r.TableId == tableId)
            .Include(r => r.Customer)
            .Include(r => r.Orders)
            .ToListAsync();
    }
}
