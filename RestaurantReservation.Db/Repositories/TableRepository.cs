﻿using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Db.Repositories;

public class TableRepository : ITableRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public TableRepository(RestaurantReservationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<bool> TableExistsAsync(int restaurantId, int id)
    {
        return await _dbContext.Tables
            .AnyAsync(c => c.RestaurantId == restaurantId
                        && c.TableId == id);
    }

    public async Task<IEnumerable<Table>> GetTablesInRestaurantAsync(int restaurantId)
    {
        return await _dbContext.Tables.Where(t => t.RestaurantId == restaurantId).ToListAsync();
    }

    public async Task<Table?> GetTableAsync(int restaurantId, int tableId)
    {
        return await _dbContext.Tables.FirstOrDefaultAsync(t => t.RestaurantId == restaurantId
                                                           && t.TableId == tableId);
    }

    public async Task<Table> CreateTableAsync(int restaurantId, Table table)
    {
        await _dbContext.Tables.AddAsync(table);
        table.RestaurantId = restaurantId;
        return table;
    }

    public void DeleteTable(Table table)
    {
        _dbContext.Tables.Remove(table);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _dbContext.SaveChangesAsync() >= 0);
    }
}