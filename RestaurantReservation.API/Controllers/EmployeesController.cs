﻿using AuthenticationService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.API.Utilities;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Api.Controllers;

[Authorize]
[Route("api/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeesController(IRestaurantRepository restaurantRepository, IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _restaurantRepository = restaurantRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all employees.
    /// </summary>
    /// <returns>The list of employees.</returns>
    /// <response code="200">Returns the list of employees.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeDto>))]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
    {
        var employees = await _employeeRepository.GetAllEmployeesAsync();
        return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
    }

    /// <summary>
    /// Gets a specific employee by ID.
    /// </summary>
    /// <param name="employeeId">The ID of the employee to retrieve.</param>
    /// <param name="includeOrders">Flag to include orders in the response.</param>
    /// <returns>The employee data transfer object.</returns>
    /// <response code="200">Returns the requested employee.</response>
    /// <response code="404">If the employee with the specified ID is not found.</response>
    [HttpGet("{employeeId}", Name = "GetEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployee(int employeeId, bool includeOrders = false)
    {
        var employee = await _employeeRepository.GetEmployeeAsync(employeeId, includeOrders);
        if (employee == null)
        {
            return NotFound(ApiErrors.EmployeeNotFound);
        }

        return Ok(includeOrders ? _mapper.Map<EmployeeWithOrdersDto>(employee) : _mapper.Map<EmployeeDto>(employee));
    }

    /// <summary>
    /// Gets all managers.
    /// </summary>
    /// <returns>The list of managers.</returns>
    /// <response code="200">Returns the list of managers.</response>
    [HttpGet("managers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeDto>))]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetManagers()
    {
        var employees = await _employeeRepository.GetManagers();
        return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
    }

    /// <summary>
    /// Gets the average order amount for a specific employee.
    /// </summary>
    /// <param name="employeeId">The ID of the employee.</param>
    /// <returns>The average order amount.</returns>
    /// <response code="200">Returns the average order amount.</response>
    /// <response code="404">If the employee with the specified ID is not found.</response>
    [HttpGet("{employeeId}/average-order-amount")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(decimal?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<decimal?>> GetAverageOrderAmountForEmployee(int employeeId)
    {
        var averageOrderAmount = await _employeeRepository.GetAverageOrderAmountAsync(employeeId);

        if (averageOrderAmount == null)
        {
            return NotFound(ApiErrors.EmployeeNotFound);
        }

        return Ok(new { AverageOrderAmount = averageOrderAmount });
    }

    /// <summary>
    /// Creates a new employee.
    /// </summary>
    /// <param name="CraetedEmployee">The data for creating a new employee.</param>
    /// <returns>The created employee.</returns>
    /// <response code="201">Returns the created employee.</response>
    /// <response code="400">If data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EmployeeDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostEmployee(EmployeeCreationDto CraetedEmployee)
    {
        var employeeEntity = _mapper.Map<Employee>(CraetedEmployee);

        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(employeeEntity.RestaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        await _employeeRepository.CreateEmployeeAsync(employeeEntity);
        var restaurant = await _restaurantRepository.GetRestaurantAsync(employeeEntity.RestaurantId);
        employeeEntity.Restaurant = restaurant;
        await _employeeRepository.SaveChangesAsync();

        return CreatedAtRoute("GetEmployee",
            new
            {
                employeeId = employeeEntity.EmployeeId
            },
            _mapper.Map<EmployeeDto>(employeeEntity));
    }

    /// <summary>
    /// Updates an existing employee.
    /// </summary>
    /// <param name="employeeId">The ID of the employee to update.</param>
    /// <param name="employeeForUpdate">The data for updating the employee.</param>
    /// <returns>No content if successful, not found if the employee does not exist.</returns>
    /// <response code="204">If the update is successful.</response>
    /// <response code="404">If the employee with the specified ID is not found.</response>
    [HttpPut("{employeeId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PutEmployee(int employeeId, EmployeeUpdateDto employeeForUpdate)
    {
        var employeeEntity = await _employeeRepository.GetEmployeeAsync(employeeId);
        if (employeeEntity == null)
        {
            return NotFound(ApiErrors.EmployeeNotFound);
        }

        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(employeeForUpdate.RestaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        _mapper.Map(employeeForUpdate, employeeEntity);
        await _employeeRepository.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific employee by ID.
    /// </summary>
    /// <param name="employeeId">The ID of the employee to delete.</param>
    /// <returns>No content if successful, not found if the employee does not exist.</returns>
    /// <response code="204">If the deletion is successful.</response>
    /// <response code="404">If the employee with the specified ID is not found.</response>
    /// <response code="400">If the employee cannot be deleted</response>
    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("{employeeId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(EmployeeDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEmployee(int employeeId)
    {
        var employeeEntity = await _employeeRepository.GetEmployeeAsync(employeeId);
        if (employeeEntity == null)
        {
            return NotFound(ApiErrors.EmployeeNotFound);
        }

        try
        {
            _employeeRepository.DeleteEmployee(employeeEntity);
            await _employeeRepository.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest(ApiErrors.DeletionIsInvalid);
        }

        return NoContent();
    }
}