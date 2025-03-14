﻿using AuthenticationService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.ModelView.Restaurant;
using RestaurantReservation.API.Utilities;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Api.Controllers;

[Authorize]
[Route("api/restaurants")]
[ApiController]
public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IMapper _mapper;

    public RestaurantsController(IRestaurantRepository restaurantRepository, IMapper mapper)
    {
        _restaurantRepository = restaurantRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all restaurants.
    /// </summary>
    /// <returns>The list of restaurants.</returns>
    /// <response code="200">Returns the list of restaurants.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RestaurantDto>))]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants()
    {
        var restaurants = await _restaurantRepository.GetAllRestaurantsAsync();
        return Ok(_mapper.Map<IEnumerable<RestaurantDto>>(restaurants));
    }

    /// <summary>
    /// Gets a specific restaurant by ID.
    /// </summary>
    /// <param name="id">The ID of the restaurant to retrieve.</param>
    /// <param name="includeEmployees">Flag to include associated employees.</param>
    /// <param name="includeMenuItems">Flag to include associated menu items.</param>
    /// <returns>The restaurant data transfer object.</returns>
    /// <response code="200">Returns the requested restaurant.</response>
    /// <response code="404">If the restaurant with the specified ID is not found.</response>
    /// <response code="400">If the client set both parameter to true.</response>

    [HttpGet("{id}", Name = "GetRestaurant")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRestaurant(int id, bool includeEmployees = false, bool includeMenuItems = false)
    {
        if (includeMenuItems && includeEmployees)
        {
            return BadRequest(ApiErrors.BigRequest);
        }

        var restaurant = await _restaurantRepository.GetRestaurantAsync(id, includeEmployees, includeMenuItems);

        if (restaurant == null)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        if (includeEmployees)
        {
            return Ok(_mapper.Map<RestaurantWithEmployeesDto>(restaurant));
        }
        else if (includeMenuItems)
        {
            return Ok(_mapper.Map<RestaurantWithMenuItemsDto>(restaurant));
        }

        return Ok(_mapper.Map<RestaurantDto>(restaurant));
    }

    /// <summary>
    /// Creates a new restaurant.
    /// </summary>
    /// <param name="restaurant">The data for creating a new restaurant.</param>
    /// <returns>The created restaurant.</returns>
    /// <response code="201">Returns the created restaurant.</response>
    /// <response code="400">If the data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RestaurantDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostRestaurant(RestaurantCreationDto restaurant)
    {
        var finalRestaurant = _mapper.Map<Restaurant>(restaurant);
        await _restaurantRepository.CreateRestaurantAsync(finalRestaurant);
        await _restaurantRepository.SaveChangesAsync();

        return CreatedAtRoute("GetRestaurant",
            new {
                id = finalRestaurant.RestaurantId
            },_mapper.Map<RestaurantDto>(finalRestaurant));
    }

    /// <summary>
    /// Updates an existing restaurant.
    /// </summary>
    /// <param name="id">The ID of the restaurant to update.</param>
    /// <param name="restaurant">The data for updating the restaurant.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">No content if successful.</response>
    /// <response code="400">If the data is invalid.</response>
    /// <response code="404">If the restaurant is not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PutRestaurant(int id, RestaurantUpdateDto restaurant)
    {
        var restaurantEntity = await _restaurantRepository.GetRestaurantAsync(id);
        if (restaurantEntity == null)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        _mapper.Map(restaurant, restaurantEntity);
        await _restaurantRepository.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing restaurant.
    /// </summary>
    /// <param name="id">The ID of the restaurant to delete.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">No content if successful.</response>
    /// <response code="400">If the restaurant cannot be deleted.</response>
    /// <response code="404">If the restaurant is not found.</response>
    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        var restaurantEntity = await _restaurantRepository.GetRestaurantAsync(id);
        if (restaurantEntity == null)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        try
        {
            _restaurantRepository.DeleteRestaurant(restaurantEntity);
            await _restaurantRepository.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest(ApiErrors.DeletionIsInvalid);
        }

        return NoContent();
    }
}