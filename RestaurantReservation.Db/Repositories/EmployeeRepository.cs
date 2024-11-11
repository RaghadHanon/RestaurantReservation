using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;
public class EmployeeRepository
{
    private RestaurantReservationDbContext _dbContext;
    public EmployeeRepository(RestaurantReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Employee?> AddEmployeeAsync(Employee employee)
    {
        if (employee == null)
            return null;

        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> RemoveEmployeeAsync(int employeeId)
    {
        if (employeeId == 0)
            return false;

        var employee = await GetEmployeeByIdAsync(employeeId);
        if (employee == null)
            return false;

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateEmployeeAsync(int employeeId, string? firstName = null, string? lastName = null, string? position = null)
    {
        if (employeeId == 0)
            return false;

        var employee = await GetEmployeeByIdAsync(employeeId);
        if (employee == null)
            return false;

        if (!firstName.IsNullOrEmpty())
            employee.FirstName = firstName;

        if (!lastName.IsNullOrEmpty())
            employee.LastName = lastName;

        if (!position.IsNullOrEmpty())
            employee.Position = position;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        return await _dbContext.Employees
                               .Include(e => e.Restaurant)
                               .Include(e => e.Orders)
                               .ToListAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
    {
        if (employeeId == 0)
            return null;

        return await _dbContext.Employees
            .Include(e => e.Restaurant)
            .Include(e => e.Orders)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
    }

    public async Task<List<Employee>> GetEmployeesByRestaurantAsync(int restaurantId)
    {
        return await _dbContext.Employees
            .Where(e => e.RestaurantId == restaurantId)
            .ToListAsync();
    }

    public async Task<List<Employee>> GetEmployeesByPositionAsync(string position)
    {
        return await _dbContext.Employees
            .Where(e => e.Position == position)
            .ToListAsync();
    }

    public async Task<List<Employee>> ListManagers()
    {
        return await _dbContext.Employees.Where(e => e.Position == "Manager").ToListAsync();
    }
}
