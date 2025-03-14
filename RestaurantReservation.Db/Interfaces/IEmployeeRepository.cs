﻿using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Interfaces;
public interface IEmployeeRepository
{
    Task<Employee> CreateEmployeeAsync(Employee employee);
    void DeleteEmployee(Employee employee);
    Task<bool> EmployeeExistsAsync(int id);
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<decimal?> GetAverageOrderAmountAsync(int employeeId);
    Task<Employee?> GetEmployeeAsync(int employeeId, bool includeOrders = false);
    Task<IEnumerable<Employee>> GetManagers();
    Task<bool> SaveChangesAsync();
}
