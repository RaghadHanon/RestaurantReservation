using AuthenticationService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.API.Utilities;
using RestaurantReservation.Db.DTOs;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Api.Controllers;

[Authorize]
[Route("api/reservations/{reservationId}/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public OrdersController(IOrderRepository orderRepository,
        IRestaurantRepository restaurantRepository,
        IReservationRepository reservationRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _restaurantRepository = restaurantRepository;
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all orders for a specific reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation.</param>
    /// <returns>The list of orders with associated menu items.</returns>
    /// <response code="200">Returns the list of orders.</response>
    /// <response code="404">If the reservation with the specified ID is not found.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<OrderWithMenuItemsDto>>> GetOrdersForReservation(int reservationId)
    {
        var reservationExists = await _reservationRepository.ReservationExistsAsync(reservationId);
        if (!reservationExists)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        var orders = await _orderRepository.GetOrdersForReservationAsync(reservationId);
        return Ok(_mapper.Map<IEnumerable<OrderWithMenuItemsDto>>(orders));
    }

    /// <summary>
    /// Gets a specific order by ID.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation.</param>
    /// <param name="orderId">The ID of the order to retrieve.</param>
    /// <returns>The order data transfer object.</returns>
    /// <response code="200">Returns the requested order.</response>
    /// <response code="404">If the reservation or order with the specified IDs is not found.</response>
    [HttpGet("{orderId}", Name = "GetOrder")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetOrder(int reservationId, int orderId)
    {
        var reservationExists = await _reservationRepository.ReservationExistsAsync(reservationId);
        if (!reservationExists)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        var order = await _orderRepository.GetOrderAsync(reservationId, orderId);
        if (order == null)
        {
            return NotFound(ApiErrors.OrderNotFound);
        }

        return Ok(_mapper.Map<OrderDto>(order));
    }

    /// <summary>
    /// Creates a new order in a specific reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation.</param>
    /// <param name="orderForCreation">The data for creating a new order.</param>
    /// <returns>The created order.</returns>
    /// <response code="201">Returns the created order.</response>
    /// <response code="404">If the reservation or order with the specified IDs is not found or Employee is not consistent with Reservation..</response>
    /// <response code="400">If data is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> PostOrder(int reservationId, OrderCreationDto orderForCreation)
    {
        var reservationExists = await _reservationRepository.ReservationExistsAsync(reservationId);
        if (!reservationExists)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        var restaurantForReservation = await _restaurantRepository.GetRestaurantIdByReservationIdAsync(reservationId);
        var restaurantForEmployee = await _restaurantRepository.GetRestaurantIdByEmployeeIdAsync(orderForCreation.EmployeeId);

        if (restaurantForReservation == null)
        {
            return NotFound($"{ApiErrors.RestaurantNotFound} for Reservation {reservationId}");
        }

        if (restaurantForEmployee == null)
        {
            return NotFound($"{ApiErrors.RestaurantNotFound} for Employee {orderForCreation.EmployeeId}");
        }

        if (restaurantForEmployee != restaurantForReservation)
        {
            return NotFound(ApiErrors.EmployeeInconsistentWithReservation);
        }

        var orderEntity = _mapper.Map<Order>(orderForCreation);
        await _orderRepository.CreateOrderAsync(reservationId, orderEntity);
        await _orderRepository.SaveChangesAsync();

        return CreatedAtRoute("GetOrder",
            new
            {
                reservationId,
                orderId = orderEntity.OrderId
            }, _mapper.Map<OrderWithoutOrderItemsDto>(orderEntity));
    }

    /// <summary>
    /// Updates an existing order in a specific reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation.</param>
    /// <param name="orderId">The ID of the order to update.</param>
    /// <param name="orderForUpdate">The data for updating the order.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">No content if successful.</response>
    /// <response code="404">If the reservation or order with the specified IDs is not found or Employee is not consistent with Reservation..</response>
    /// <response code="400">If data is invalid</response>
    [HttpPut("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PutOrder(int reservationId, int orderId, OrderUpdateDto orderForUpdate)
    {
        var reservationExists = await _reservationRepository.ReservationExistsAsync(reservationId);
        if (!reservationExists)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        var orderEntity = await _orderRepository.GetOrderAsync(reservationId, orderId);
        if (orderEntity == null)
        {
            return NotFound(ApiErrors.OrderNotFound);
        }

        var restaurantForReservation = await _restaurantRepository.GetRestaurantIdByReservationIdAsync(orderForUpdate.ReservationId);
        var restaurantForEmployee = await _restaurantRepository.GetRestaurantIdByEmployeeIdAsync(orderForUpdate.EmployeeId);

        if (restaurantForReservation == null)
        {
            return NotFound($"{ApiErrors.RestaurantNotFound} for Reservation {orderForUpdate.ReservationId}");
        }

        if (restaurantForEmployee == null)
        {
            return NotFound($"{ApiErrors.RestaurantNotFound} for Employee {orderForUpdate.EmployeeId}");
        }

        if (restaurantForEmployee != restaurantForReservation)
        {
            return NotFound(ApiErrors.EmployeeInconsistentWithReservation);
        }

        _mapper.Map(orderForUpdate, orderEntity);
        await _orderRepository.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing order in a specific reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation.</param>
    /// <param name="orderId">The ID of the order to delete.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">No content if successful.</response>
    /// <response code="404">If the reservation or order with the specified IDs is not found.</response>
    /// <response code="400">If the order cannot be deleted</response>

    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteOrder(int reservationId, int orderId)
    {
        var reservationExists = await _reservationRepository.ReservationExistsAsync(reservationId);
        if (!reservationExists)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        var orderEntity = await _orderRepository.GetOrderAsync(reservationId, orderId);
        if (orderEntity == null)
        {
            return NotFound(ApiErrors.OrderNotFound);
        }

        try
        {
            _orderRepository.DeleteOrder(orderEntity);
            await _orderRepository.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest(ApiErrors.DeletionIsInvalid);
        }

        return NoContent();
    }
}