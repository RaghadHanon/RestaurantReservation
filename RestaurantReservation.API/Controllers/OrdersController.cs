using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.API.Utilities;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.API.Controllers;
[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IReservationRepository _reservationRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public OrdersController(IOrderRepository orderRepository, IEmployeeRepository employeeRepository, IReservationRepository reservationRepository, IMapper mapper)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderRepository.GetOrdersAsync();
        var ordersToReturn = _mapper.Map<IEnumerable<OrderDto>>(orders);
        return Ok(ordersToReturn);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _orderRepository.GetOrderAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        var orderToReturn = _mapper.Map<OrderDto>(order);
        return Ok(orderToReturn);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder(OrderCreationDto orderDto)
    {
        if (orderDto == null)
        {
            return BadRequest(Errors.DataIsNull);
        }

        var reservation = await _reservationRepository.GetReservationAsync(orderDto.ReservationId);
        if (reservation == null)
        {
            return BadRequest(Errors.DataIsInvalid);
        }

        var employee = await _employeeRepository.GetEmployeeAsync(orderDto.EmployeeId);
        if (employee == null)
        {
            return BadRequest(Errors.DataIsInvalid);
        }

        var order = _mapper.Map<Order>(orderDto);
        order.Reservation = reservation;
        order.Employee = employee;
        await _orderRepository.AddOrderAsync(order);
        await _orderRepository.SaveChangesAsync();
        var createdOrderToReturn = _mapper.Map<OrderDto>(order);
        return CreatedAtAction(nameof(GetOrder),
            new {
                id = order.OrderId 
            }, createdOrderToReturn);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, OrderUpdateDto updatedOrderDto)
    {
        if (updatedOrderDto == null)
        {
            return BadRequest(Errors.DataIsNull);
        }

        var order = await _orderRepository.GetOrderAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        _mapper.Map(updatedOrderDto, order);
        await _orderRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _orderRepository.GetOrderAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        _orderRepository.RemoveOrderAsync(order);
        await _orderRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{reservationId}/OrderedMenuItems")]
    public async Task<IActionResult> GetOrderItemsByReservation(int reservationId)
    {
        var orderItems = await _orderRepository.ListOrderedMenuItems(reservationId);
        return Ok(orderItems);
    }

    [HttpGet("{reservationId}/OrderWithMenuItems")]
    public async Task<IActionResult> GetOrdersWithMenuItems(int reservationId)
    {
        var orders = await _orderRepository.ListOrdersAndMenuItems(reservationId);
        return Ok(orders);
    }
}
