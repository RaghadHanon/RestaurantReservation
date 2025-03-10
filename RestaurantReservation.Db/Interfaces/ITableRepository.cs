﻿using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Interfaces
{
    public interface ITableRepository
    {
        Task<Table> CreateTableAsync(int restaurantId, Table table);
        void DeleteTable(Table table);
        Task<Table?> GetTableAsync(int restaurantId, int tableId);
        Task<IEnumerable<Table>> GetTablesInRestaurantAsync(int restaurantId);
        Task<bool> SaveChangesAsync();
        Task<bool> TableExistsAsync(int restaurantId, int id);
    }
}