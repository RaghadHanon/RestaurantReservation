﻿using AuthenticationService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.API.Utilities;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Api.Controllers;

[Authorize]
[Route("api/reservations/{reservationId}/orders/{orderId}/orderitems")]
[ApiController]
public class OrderItemsController : ControllerBase
{
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;
    private readonly IReservationRepository _reservationRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IRestaurantRepository _restaurantRepository;

    public OrderItemsController(IOrderItemRepository orderItemRepository,
        IReservationRepository reservationRepository,
        IOrderRepository orderRepository,
        IRestaurantRepository restaurantRepository,
        IMapper mapper)
    {
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
        _reservationRepository = reservationRepository;
        _orderRepository = orderRepository;
        _restaurantRepository = restaurantRepository;
    }

    /// <summary>
    /// Gets all order items for a specific order in a reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation.</param>
    /// <param name="orderId">The ID of the order.</param>
    /// <returns>The list of order items.</returns>
    /// <response code="200">Returns the list of order items.</response>
    /// <response code="404">If the reservation or order with the specified IDs is not found.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderItemDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItemsForOrder(int reservationId, int orderId)
    {
        var reservationExists = await _reservationRepository.ReservationExistsAsync(reservationId);
        var orderExists = await _orderRepository.OrderExistsAsync(reservationId, orderId);
        if (!reservationExists)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        if (!orderExists)
        {
            return NotFound(ApiErrors.OrderNotFound);
        }

        var orderItems = await _orderItemRepository.GetOrderItemsForOrderAsync(reservationId, orderId);
        return Ok(_mapper.Map<IEnumerable<OrderItemDto>>(orderItems));
    }

    /// <summary>
    /// Gets a specific order item by ID for a specific order in a reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation.</param>
    /// <param name="orderId">The ID of the order.</param>
    /// <param name="orderItemId">The ID of the order item to retrieve.</param>
    /// <returns>The order item data transfer object.</returns>
    /// <response code="200">Returns the requested order item.</response>
    /// <response code="404">If the reservation, order, or order item with the specified IDs is not found.</response>
    [HttpGet("{orderItemId}", Name = "GetOrderItem")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderItemDto>> GetOrderItem(int reservationId, int orderId, int orderItemId)
    {
        var reservationExists = await _reservationRepository.ReservationExistsAsync(reservationId);
        var orderExists = await _orderRepository.OrderExistsAsync(reservationId, orderId);

        if (!reservationExists)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        if (!orderExists)
        {
            return NotFound(ApiErrors.OrderNotFound);
        }

        var orderItem = await _orderItemRepository.GetOrderItemAsync(reservationId, orderId, orderItemId);
        if (orderItem == null)
        {
            return NotFound(ApiErrors.OrderItemNotFound);
        }

        return Ok(_mapper.Map<OrderItemDto>(orderItem));
    }

    /// <summary>
    /// Creates a new order item for a specific order in a reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation.</param>
    /// <param name="orderId">The ID of the order.</param>
    /// <param name="orderItemForCreation">The data for creating a new order item.</param>
    /// <returns>The created order item.</returns>
    /// <response code="201">Returns the created order item.</response>
    /// <response code="404">If the reservation or order with the specified IDs is not found or Reservation is not consistent with MenuItem.</response>
    /// <response code="400">If data is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderItemDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderItemDto>> PostOrderItem(int reservationId, int orderId, OrderItemCreationDto orderItemForCreation)
    {
        var reservationExists = await _reservationRepository.ReservationExistsAsync(reservationId);
        var orderExists = await _orderRepository.OrderExistsAsync(reservationId, orderId);

        if (!reservationExists)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        if (!orderExists)
        {
            return NotFound(ApiErrors.OrderNotFound);
        }

        var restaurantForReservation = await _restaurantRepository.GetRestaurantIdByReservationIdAsync(reservationId);
        var restaurantForMenuItem = await _restaurantRepository.GetRestaurantIdByMenuItemIdAsync(orderItemForCreation.ItemId);

        if (restaurantForReservation == null)
        {
            return NotFound($"{ApiErrors.RestaurantNotFound} for reservation {reservationId}");
        }

        if (restaurantForMenuItem == null)
        {
            return NotFound($"{ApiErrors.RestaurantNotFound} for MenuItem {orderItemForCreation.ItemId}");
        }

        if (restaurantForMenuItem != restaurantForReservation)
        {
            return NotFound(ApiErrors.ReservationInconsistentWithMenuItem);
        }

        var orderItemEntity = _mapper.Map<OrderItem>(orderItemForCreation);
        await _orderItemRepository.AddOrderItemToOrderAsync(orderId, orderItemEntity);
        await _orderItemRepository.SaveChangesAsync(orderId);

        return CreatedAtRoute("GetOrderItem",
            new {
                reservationId,
                orderId,
                orderItemId = orderItemEntity.OrderItemId 
            }, _mapper.Map<OrderItemDto>(orderItemEntity));
    }

    /// <summary>
    /// Updates an existing order item for a specific order in a reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation.</param>
    /// <param name="orderId">The ID of the order.</param>
    /// <param name="orderItemId">The ID of the order item to update.</param>
    /// <param name="orderItemForUpdate">The data for updating the order item.</param>
    /// <returns>No content if successful, not found if the reservation, order, or order item does not exist.</returns>
    /// <response code="204">If the update is successful.</response>
    /// <response code="400">If data is invalid</response>
    /// <response code="404">If the reservation, order ,order item with the specified ID is not found or Reservation is not consistent with MenuItem.</response>
    [HttpPut("{orderItemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PutOrderItem(int reservationId, int orderId, int orderItemId,
        OrderItemUpdateDto orderItemForUpdate)
    {
        var reservationExists = await _reservationRepository.ReservationExistsAsync(reservationId);
        var orderExists = await _orderRepository.OrderExistsAsync(reservationId, orderId);

        if (!reservationExists)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        if (!orderExists)
        {
            return NotFound(ApiErrors.OrderNotFound);
        }

        var orderItemEntity = await _orderItemRepository.GetOrderItemAsync(reservationId, orderId, orderItemId);
        if (orderItemEntity == null)
        {
            return NotFound(ApiErrors.OrderItemNotFound);
        }

        var restaurantForReservation = await _restaurantRepository.GetRestaurantIdByReservationIdAsync(reservationId);
        var restaurantForMenuItem = await _restaurantRepository.GetRestaurantIdByMenuItemIdAsync(orderItemForUpdate.ItemId);

        if (restaurantForReservation == null)
        {

            return NotFound($"{ApiErrors.RestaurantNotFound} for reservation {reservationId}");
        }

        if (restaurantForMenuItem == null)
        {

            return NotFound($"{ApiErrors.RestaurantNotFound} for MenuItem {orderItemEntity.ItemId}");
        }

        if (restaurantForMenuItem != restaurantForReservation)
        {
            return NotFound(ApiErrors.ReservationInconsistentWithMenuItem);
        }

        _mapper.Map(orderItemForUpdate, orderItemEntity);
        await _orderItemRepository.SaveChangesAsync(orderId);

        return NoContent();
    }

    /// <summary>
    /// Deletes a specific order item by ID for a specific order in a reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation.</param>
    /// <param name="orderId">The ID of the order.</param>
    /// <param name="orderItemId">The ID of the order item to delete.</param>
    /// <returns>No content if successful, not found if the reservation, order, or order item does not exist.</returns>
    /// <response code="204">If the deletion is successful.</response>
    /// <response code="404">If the reservation, order, or order item with the specified IDs is not found.</response>
    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("{orderItemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrderItem(int reservationId, int orderId, int orderItemId)
    {
        var reservationExists = await _reservationRepository.ReservationExistsAsync(reservationId);
        var orderExists = await _orderRepository.OrderExistsAsync(reservationId, orderId);

        if (!reservationExists)
        {
            return NotFound(ApiErrors.ReservationNotFound);
        }

        if (!orderExists)
        {
            return NotFound(ApiErrors.OrderNotFound);
        }

        var orderItemEntity = await _orderItemRepository.GetOrderItemAsync(reservationId, orderId, orderItemId);
        if (orderItemEntity == null)
        {
            return NotFound(ApiErrors.OrderItemNotFound);
        }

        _orderItemRepository.DeleteOrderItem(orderItemEntity);
        await _orderItemRepository.SaveChangesAsync(orderId);

        return NoContent();
    }
}