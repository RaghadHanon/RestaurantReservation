﻿using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;
public class OrderRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public OrderRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> AddOrderAsync(Order order)
    {
        if (order == null)
            return null;

        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
        return order;
    }

    public async Task<bool> RemoveOrderAsync(int orderId)
    {
        if (orderId == 0)
            return false;

        var order = await GetOrderByIdAsync(orderId);
        if (order == null)
            return false;

        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateOrderAsync(int orderId, DateTime? orderDate = null, decimal? totalAmount = null)
    {
        if (orderId == 0)
            return false;

        var order = await GetOrderByIdAsync(orderId);
        if (order == null)
            return false;

        if (orderDate.HasValue)
            order.OrderDate = orderDate.Value;

        if (totalAmount.HasValue)
            order.TotalAmount = totalAmount.Value;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _dbContext.Orders
            .Include(o => o.Employee)
            .Include(o => o.Reservation)
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        if (orderId == 0)
            return null;

        return await _dbContext.Orders
            .Include(o => o.Employee)
            .Include(o => o.Reservation)
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<List<Order>> GetOrdersByEmployeeAsync(int employeeId)
    {
        return await _dbContext.Orders
            .Where(o => o.EmployeeId == employeeId)
            .Include(o => o.Employee)
            .Include(o => o.Reservation)
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByReservationAsync(int reservationId)
    {
        return await _dbContext.Orders
            .Where(o => o.ReservationId == reservationId)
            .Include(o => o.Employee)
            .Include(o => o.Reservation)
            .Include(o => o.OrderItems)
            .ToListAsync();
    }
}