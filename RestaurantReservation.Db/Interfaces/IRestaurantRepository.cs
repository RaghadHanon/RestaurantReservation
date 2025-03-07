﻿using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Interfaces;
public interface IRestaurantRepository
{
    Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant);
    void DeleteRestaurant(Restaurant restaurant);
    Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
    Task<Restaurant?> GetRestaurantAsync(int id, bool includeEmployees = false, bool includeMenuItems = false);
    Task<int?> GetRestaurantIdByEmployeeIdAsync(int employeeId);
    Task<int?> GetRestaurantIdByMenuItemIdAsync(int menuItemId);
    Task<int?> GetRestaurantIdByReservationIdAsync(int reservationId);
    Task<bool> RestaurantExistsAsync(int id);
    Task<bool> SaveChangesAsync();
}
