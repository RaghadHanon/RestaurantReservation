﻿using RestaurantReservation.API.Models.OrderItem;

namespace RestaurantReservation.API.Models.Order;
public class OrderDto
{
    public int OrderId { get; set; }
    public int ReservationId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
}