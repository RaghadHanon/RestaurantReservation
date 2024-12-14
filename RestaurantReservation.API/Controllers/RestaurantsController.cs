using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.API.ModelView.Restaurant;
using RestaurantReservation.API.ModelView.Table;
using RestaurantReservation.API.Utilities;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.API.Controllers;

[Route("api/restaurants")]
[ApiController]
public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IMapper _mapper;

    public RestaurantsController(IRestaurantRepository restaurantRepository, IMapper mapper)
    {
        _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<IActionResult> GetRestaurants()
    {
        var restaurants = await _restaurantRepository.GetRestaurantsAsync();
        var restaurantsToReturn = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        return Ok(restaurantsToReturn);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRestaurant(int id)
    {
        var restaurant = await _restaurantRepository.GetRestaurantAsync(id);
        if (restaurant == null)
        {
            return NotFound();
        }

        var restaurantToReturn = _mapper.Map<RestaurantDto>(restaurant);
        return Ok(restaurantToReturn);
    }

    [HttpPost]
    public async Task<IActionResult> AddRestaurant(RestaurantCreationDto restaurantDto)
    {
        if (restaurantDto == null)
        {
            return BadRequest(Errors.DataIsNull);
        }

        var newRestaurant = _mapper.Map<Restaurant>(restaurantDto);
        await _restaurantRepository.AddRestaurantAsync(newRestaurant);
        await _restaurantRepository.SaveChangesAsync();

        var createdRestaurantToReturn = _mapper.Map<RestaurantDto>(newRestaurant);
        return CreatedAtAction(nameof(GetRestaurant), new { id = newRestaurant.RestaurantId }, createdRestaurantToReturn);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRestaurant(int id, RestaurantUpdateDto restaurantDto)
    {
        if (restaurantDto == null)
        {
            return BadRequest(Errors.DataIsNull);
        }

        var restaurant = await _restaurantRepository.GetRestaurantAsync(id);
        if (restaurant == null)
        {
            return NotFound();
        }

        _mapper.Map(restaurantDto, restaurant);
        await _restaurantRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        var restaurant = await _restaurantRepository.GetRestaurantAsync(id);
        if (restaurant == null)
        {
            return NotFound();
        }

        _restaurantRepository.RemoveRestaurantAsync(restaurant);
        await _restaurantRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id}/revenue")]
    public async Task<IActionResult> GetTotalRevenue(int id)
    {
        var revenue = await _restaurantRepository.GetTotalRevenueAsync(id);
        return Ok(new { Revenue = revenue });
    }

    [HttpGet("{id}/employees")]
    public async Task<IActionResult> GetEmployeesByRestaurantId(int id)
    {
        var employees = await _restaurantRepository.GetEmployeesByRestaurantIdAsync(id);
        var employeesToReturn = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return Ok(employeesToReturn);
    }

    [HttpGet("{id}/tables")]
    public async Task<IActionResult> GetTablesByRestaurantId(int id)
    {
        var tables = await _restaurantRepository.GetTablesByRestaurantIdAsync(id);
        var tablesToReturn = _mapper.Map<IEnumerable<TableDto>>(tables);
        return Ok(tablesToReturn);
    }

    [HttpGet("{id}/menu-items")]
    public async Task<IActionResult> GetMenuItemsByRestaurantId(int id)
    {
        var menuItems = await _restaurantRepository.GetMenuItemsByRestaurantIdAsync(id);
        var menuItemsToReturn = _mapper.Map<IEnumerable<MenuItemDto>>(menuItems);
        return Ok(menuItemsToReturn);
    }

    [HttpGet("{id}/reservations")]
    public async Task<IActionResult> GetReservationsByRestaurantId(int id)
    {
        var reservations = await _restaurantRepository.GetReservationsByRestaurantIdAsync(id);
        var reservationsToReturn = _mapper.Map<IEnumerable<ReservationDto>>(reservations);
        return Ok(reservationsToReturn);
    }
}
