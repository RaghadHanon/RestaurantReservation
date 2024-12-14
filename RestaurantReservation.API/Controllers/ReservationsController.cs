using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.API.Utilities;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.API.Controllers;
[Route("api/reservations")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IMapper _mapper;

    public ReservationsController(
        IReservationRepository reservationRepository,
        ICustomerRepository customerRepository,
        IRestaurantRepository restaurantRepository,
        IMapper mapper)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<IActionResult> GetReservations(bool includeOrders = false)
    {
        var reservations = await _reservationRepository.GetReservationsAsync(includeOrders);
        if (includeOrders)
        {
            var reservationsWithOrders = _mapper.Map<IEnumerable<ReservationWithOrdersDto>>(reservations);
            return Ok(reservationsWithOrders);
        }

        var reservationsToReturn = _mapper.Map<IEnumerable<ReservationDto>>(reservations);
        return Ok(reservationsToReturn);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservation(int id, bool includeOrders = false)
    {
        var reservation = await _reservationRepository.GetReservationAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }

        if (includeOrders)
        {
            var reservationWithOrders = _mapper.Map<ReservationWithOrdersDto>(reservation);
            return Ok(reservationWithOrders);
        }

        var reservationToReturn = _mapper.Map<ReservationDto>(reservation);
        return Ok(reservationToReturn);
    }

    [HttpPost]
    public async Task<IActionResult> AddReservation(ReservationCreationDto reservationDto)
    {
        if (reservationDto == null)
        {
            return BadRequest(ApiErrors.DataIsNull);
        }

        var customer = await _customerRepository.GetCustomerAsync(reservationDto.CustomerId);
        if (customer == null)
        {
            return BadRequest(ApiErrors.DataIsInvalid);
        }

        var restaurant = await _restaurantRepository.GetRestaurantAsync(reservationDto.RestaurantId);
        if (restaurant == null)
        {
            return BadRequest(ApiErrors.DataIsInvalid);
        }

        var reservation = _mapper.Map<Reservation>(reservationDto);
        reservation.Customer = customer;
        reservation.Restaurant = restaurant;

        await _reservationRepository.AddReservationAsync(reservation);
        await _reservationRepository.SaveChangesAsync();

        var createdReservationToReturn = _mapper.Map<ReservationDto>(reservation);
        return CreatedAtAction(nameof(GetReservation),
            new {
                id = reservation.ReservationId 
            }, createdReservationToReturn);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReservation(int id, ReservationUpdateDto reservationDto)
    {
        if (reservationDto == null)
        {
            return BadRequest(ApiErrors.DataIsNull);
        }

        var reservation = await _reservationRepository.GetReservationAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }

        _mapper.Map(reservationDto, reservation);
        await _reservationRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        var reservation = await _reservationRepository.GetReservationAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }

        _reservationRepository.RemoveReservationAsync(reservation);
        await _reservationRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetReservationsByCustomer(int customerId)
    {
        var reservations = await _reservationRepository.GetReservationsByCustomerIdAsync(customerId);
        if (!reservations.Any())
        {
            return NotFound();
        }

        var reservationsToReturn = _mapper.Map<IEnumerable<ReservationDto>>(reservations);
        return Ok(reservationsToReturn);
    }

    [HttpGet("{reservationId}/orders")]
    public async Task<IActionResult> GetOrdersByReservation(int reservationId)
    {
        var reservation = await _reservationRepository.GetReservationAsync(reservationId);
        if (reservation == null || reservation.Orders == null)
        {
            return NotFound();
        }

        var ordersToReturn = _mapper.Map<IEnumerable<OrderDto>>(reservation.Orders);
        return Ok(ordersToReturn);
    }

    [HttpGet("{reservationId}/menu-items")]
    public async Task<IActionResult> GetMenuItemsByReservation(int reservationId)
    {
        var reservation = await _reservationRepository.GetReservationAsync(reservationId);
        if (reservation == null || reservation.Orders == null)
        {
            return NotFound();
        }

        var menuItems = reservation.Orders.SelectMany(o => o.OrderItems).Select(oi => oi.MenuItem);
        var menuItemsToReturn = _mapper.Map<IEnumerable<MenuItemDto>>(menuItems);
        return Ok(menuItemsToReturn);
    }
}

