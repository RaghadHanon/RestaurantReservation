﻿using AuthenticationService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.Utilities;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Api.Controllers;
[Authorize]
[Route("api/restaurants/{restaurantId}/menuitems")]
[ApiController]
public class MenuItemsController : ControllerBase
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IMapper _mapper;

    public MenuItemsController(IRestaurantRepository restaurantRepository, IMenuItemRepository menuItemRepository, IMapper mapper)
    {
        _restaurantRepository = restaurantRepository;
        _menuItemRepository = menuItemRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all menu items in a restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <returns>The list of menu items.</returns>
    /// <response code="200">Returns the list of menu items.</response>
    /// <response code="404">If the restaurant with the specified ID is not found.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MenuItemDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItems(int restaurantId)
    {
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(restaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        var menuItems = await _menuItemRepository.GetMenuItemsInRestaurantAsync(restaurantId);
        return Ok(_mapper.Map<IEnumerable<MenuItemDto>>(menuItems));
    }

    /// <summary>
    /// Gets a specific menu item by ID.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <param name="menuItemId">The ID of the menu item to retrieve.</param>
    /// <returns>The menu item data transfer object.</returns>
    /// <response code="200">Returns the requested menu item.</response>
    /// <response code="404">If the restaurant or menu item with the specified IDs is not found.</response>
    [HttpGet("{menuItemId}", Name = "GetMenuItem")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuItemDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMenuItem(int restaurantId, int menuItemId)
    {
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(restaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        var menuItem = await _menuItemRepository.GetMenuItemAsync(restaurantId, menuItemId);
        if (menuItem == null)
        {
            return NotFound(ApiErrors.MenuItemNotFound);
        }

        return Ok(_mapper.Map<MenuItemDto>(menuItem));
    }

    /// <summary>
    /// Creates a new menu item in a restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <param name="menuItemForCreation">The data for creating a new menu item.</param>
    /// <returns>The created menu item.</returns>
    /// <response code="201">Returns the created menu item.</response>
    /// <response code="400">If the restaurant with the specified ID is not found or the data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MenuItemDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostMenuItem(int restaurantId, MenuItemCreationDto menuItemForCreation)
    {
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(restaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        var menuItemEntity = _mapper.Map<MenuItem>(menuItemForCreation);
        var createdMenuItem = await _menuItemRepository.CreateMenuItemAsync(restaurantId, menuItemEntity);
        await _menuItemRepository.SaveChangesAsync();

        return CreatedAtRoute("GetMenuItem",
            new { 
                restaurantId, menuItemId = createdMenuItem.MenuItemId 
            },
            _mapper.Map<MenuItemDto>(createdMenuItem));
    }

    /// <summary>
    /// Updates an existing menu item in a restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <param name="menuItemId">The ID of the menu item to update.</param>
    /// <param name="menuItemForUpdate">The data for updating the menu item.</param>
    /// <returns>No content if successful, not found if the restaurant or menu item does not exist.</returns>
    /// <response code="204">If the update is successful.</response>
    /// <response code="400">if the data is invalid, or the put fails.</response>
    /// <response code="404">If the menu item with the specified ID or If the restaurant with the specified ID or target restaurant with the specified ID is not found.</response>
    [HttpPut("{menuItemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PutMenuItem(int restaurantId, int menuItemId, MenuItemUpdateDto menuItemForUpdate)
    {
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(restaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        var menuItemEntity = await _menuItemRepository.GetMenuItemAsync(restaurantId, menuItemId);
        if (menuItemEntity == null)
        {
            return NotFound(ApiErrors.MenuItemNotFound);
        }

        var targetRestaurantExists = await _restaurantRepository.RestaurantExistsAsync(menuItemForUpdate.RestaurantId);
        if (!targetRestaurantExists)
        {
            return NotFound($"Target {ApiErrors.RestaurantNotFound}");
        }

        _mapper.Map(menuItemForUpdate, menuItemEntity);
        await _menuItemRepository.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific menu item by ID.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <param name="menuItemId">The ID of the menu item to delete.</param>
    /// <returns>No content if successful, not found if the restaurant or menu item does not exist.</returns>
    /// <response code="204">If the deletion is successful.</response>
    /// <response code="404">If the restaurant or menu item with the specified IDs is not found.</response>
    /// <response code="400">If the menu item cannot be deleted</response>
    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("{menuItemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMenuItem(int restaurantId, int menuItemId)
    {
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(restaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        var menuItemEntity = await _menuItemRepository.GetMenuItemAsync(restaurantId, menuItemId);
        if (menuItemEntity == null)
        {
            return NotFound(ApiErrors.MenuItemNotFound);
        }

        try
        {
            _menuItemRepository.DeleteMenuItem(menuItemEntity);
            await _menuItemRepository.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest(ApiErrors.DeletionIsInvalid);
        }
        return NoContent();
    }
}