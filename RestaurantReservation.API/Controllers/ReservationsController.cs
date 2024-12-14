using AuthenticationService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.API.Utilities;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Api.Controllers;

[Authorize]
[Route("api/reservations")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ITableRepository _tableRepository;
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IMapper _mapper;

    public ReservationsController(
        IReservationRepository reservationRepository,
        ICustomerRepository customerRepository,
        IRestaurantRepository restaurantRepository,
        ITableRepository tableRepository,
        IMenuItemRepository menuItemRepository,
        IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _customerRepository = customerRepository;
        _restaurantRepository = restaurantRepository;
        _tableRepository = tableRepository;
        _menuItemRepository = menuItemRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all reservations.
    /// </summary>
    /// <returns>The list of reservations.</returns>
    /// <response code="200">Returns the list of reservations.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReservationDto>))]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
    {
        var reservations = await _reservationRepository.GetAllReservationsAsync();
        return Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations));
    }

    /// <summary>
    /// Gets a specific reservation by ID.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation to retrieve.</param>
    /// <param name="includeOrders">Flag to include associated orders.</param>
    /// <returns>The reservation data transfer object.</returns>
    /// <response code="200">Returns the requested reservation.</response>
    /// <response code="404">If the reservation with the specified ID is not found.</response>
    [HttpGet("{reservationId}", Name = "GetReservation")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReservation(int reservationId, bool includeOrders = false)
    {
        var reservation = await _reservationRepository.GetReservationAsync(reservationId, includeOrders);
        if (reservation == null)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        if (includeOrders)
        {
            return Ok(_mapper.Map<ReservationWithOrdersDto>(reservation));
        }

        return Ok(_mapper.Map<ReservationDto>(reservation));
    }

    /// <summary>
    /// Gets all reservations for a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>The list of reservations for the customer.</returns>
    /// <response code="200">Returns the list of reservations for the customer.</response>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReservationDto>))]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservation(int customerId)
    {
        var reservations = await _reservationRepository.GetReservationsByCustomerIdAsync(customerId);
        return Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations));
    }

    /// <summary>
    /// Gets all ordered menu items for a specific reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation.</param>
    /// <returns>The list of ordered menu items.</returns>
    /// <response code="200">Returns the list of ordered menu items.</response>
    [HttpGet("{reservationId}/menu-items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MenuItemDto>))]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetOrderedMenuItemsForReservation(int reservationId)
    {
        var orderedMenuItems = await _menuItemRepository.GetOrderedMenuItemsByReservationIdAsync(reservationId);
        return Ok(_mapper.Map<IEnumerable<MenuItemDto>>(orderedMenuItems));
    }

    /// <summary>
    /// Creates a new reservation.
    /// </summary>
    /// <param name="reservationForCreation">The data for creating a new reservation.</param>
    /// <returns>The created reservation.</returns>
    /// <response code="201">Returns the created reservation.</response>
    /// <response code="400">If the data is invalid</response>
    /// <response code="404"> if related entities (customer, restaurant, table) do not exist.</response>

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReservationDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostReservation(ReservationCreationDto reservationForCreation)
    {
        var customerExists = await _customerRepository.CustomerExistsAsync(reservationForCreation.CustomerId);
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(reservationForCreation.RestaurantId);
        var tableExists = await _tableRepository.TableExistsAsync(reservationForCreation.RestaurantId, reservationForCreation.TableId);

        if (!customerExists)
        {
            return NotFound(ApiErrors.CustomerNotFound);
        }

        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        if (!tableExists)
        {
            return NotFound(ApiErrors.TableNotFound);
        }

        var reservationEntity = _mapper.Map<Reservation>(reservationForCreation);
        await _reservationRepository.CreateReservationAsync(reservationEntity);
        await _reservationRepository.SaveChangesAsync();

        return CreatedAtRoute("GetReservation",
            new {
                reservationId = reservationEntity.ReservationId 
            },_mapper.Map<ReservationDto>(reservationEntity));
    }

    /// <summary>
    /// Updates an existing reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation to update.</param>
    /// <param name="reservationForUpdate">The data for updating the reservation.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="201">Returns the created order.</response>
    /// <response code="400">If the data is invalid</response>
    /// <response code="404"> If reservation is not found or if related entities (customer, restaurant, table) do not exist.</response>
    [HttpPut("{reservationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PutReservation(int reservationId, ReservationUpdateDto reservationForUpdate)
    {
        var reservationEntity = await _reservationRepository.GetReservationAsync(reservationId);
        if (reservationEntity == null)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        var customerExists = await _customerRepository.CustomerExistsAsync(reservationForUpdate.CustomerId);
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(reservationForUpdate.RestaurantId);
        var tableExists = await _tableRepository.TableExistsAsync(reservationForUpdate.RestaurantId, reservationForUpdate.TableId);

        if (!customerExists)
        {
            return NotFound(ApiErrors.CustomerNotFound);
        }

        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        if (!tableExists)
        {
            return NotFound(ApiErrors.TableNotFound);
        }

        _mapper.Map(reservationForUpdate, reservationEntity);
        await _reservationRepository.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation to delete.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">No content if successful.</response>
    /// <response code="400">If the reservation cannot be deleted.</response>
    /// <response code="404"> If reservation is not found.</response>
    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("{reservationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReservation(int reservationId)
    {
        var reservationEntity = await _reservationRepository.GetReservationAsync(reservationId);
        if (reservationEntity == null)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        try
        {
            _reservationRepository.DeleteReservation(reservationEntity);
            await _reservationRepository.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest(ApiErrors.DeletionIsInvalid);
        }

        return NoContent();
    }
}