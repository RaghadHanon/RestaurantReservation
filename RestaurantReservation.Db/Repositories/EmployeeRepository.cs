using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Db.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly RestaurantReservationDbContext _dbContext;

    public EmployeeRepository(RestaurantReservationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<bool> EmployeeExistsAsync(int id)
    {
        return await _dbContext.Employees.AnyAsync(e => e.EmployeeId == id);
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await _dbContext.Employees.Include(e => e.Restaurant).ToListAsync();
    }

    public async Task<Employee?> GetEmployeeAsync(int employeeId, bool includeOrders = false)
    {
        var query = _dbContext.Employees.Include(e => e.Restaurant).AsQueryable();
        if (includeOrders)
        {
            query = query.Include(e => e.Orders);
        }

        return await query.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
    }

    public async Task<IEnumerable<Employee>> GetManagers()
    {
        return await _dbContext.Employees
            .Include(e => e.Restaurant)
            .Where(e => e.Position == "Manager")
            .ToListAsync();
    }

    public async Task<decimal?> GetAverageOrderAmountAsync(int employeeId)
    {
        var employee = await _dbContext.Employees
            .Include(e => e.Orders)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

        if (employee == null)
        {
            return null;
        }

        if (!employee.Orders.Any())
        {
            return 0;
        }

        var averageOrderAmount = employee.Orders
            .Average(order => order.TotalAmount);

        return averageOrderAmount;
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        await _dbContext.Employees.AddAsync(employee);
        return employee;
    }

    public void DeleteEmployee(Employee employee)
    {
        _dbContext.Employees.Remove(employee);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _dbContext.SaveChangesAsync() >= 0);
    }
}