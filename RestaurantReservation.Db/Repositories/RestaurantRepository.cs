﻿using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Db.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly RestaurantReservationDbContext _context;

    public RestaurantRepository(RestaurantReservationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RestaurantExistsAsync(int id)
    {
        return await _context.Restaurants.AnyAsync(c => c.RestaurantId == id);
    }
    public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
    {
        return await _context.Restaurants.ToListAsync();
    }

    public async Task<Restaurant?> GetRestaurantAsync(int id, bool includeEmployees = false,
        bool includeMenuItems = false)
    {
        var restaurant = await _context.Restaurants.FirstOrDefaultAsync(c => c.RestaurantId == id);
        if (restaurant == null)
        {
            return null;
        }

        if (includeEmployees)
        {
            await _context.Entry(restaurant).Collection(r => r.Employees).LoadAsync();
        }

        if (includeMenuItems)
        {
            await _context.Entry(restaurant).Collection(r => r.MenuItems).LoadAsync();
        }

        return restaurant;
    }
    public async Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant)
    {
        await _context.Restaurants.AddAsync(restaurant);
        return restaurant;
    }

    public void DeleteRestaurant(Restaurant restaurant)
    {
        _context.Restaurants.Remove(restaurant);
    }

    public async Task<int?> GetRestaurantIdByMenuItemIdAsync(int menuItemId)
    {
        return await _context.MenuItems
            .Where(mi => mi.MenuItemId == menuItemId)
            .Select(mi => mi.RestaurantId)
            .FirstOrDefaultAsync();
    }

    public async Task<int?> GetRestaurantIdByReservationIdAsync(int reservationId)
    {
        return await _context.Reservations
            .Where(r => r.ReservationId == reservationId)
            .Select(r => r.RestaurantId)
            .FirstOrDefaultAsync();
    }

    public async Task<int?> GetRestaurantIdByEmployeeIdAsync(int employeeId)
    {
        return await _context.Employees
            .Where(e => e.EmployeeId == employeeId)
            .Select(e => e.RestaurantId)
            .FirstOrDefaultAsync();
    }


    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}