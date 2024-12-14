﻿using RestaurantReservation.API.Models.OrderItem;

namespace RestaurantReservation.API.Models.Order;
public class OrderWithoutOrderItemsDto
{
    public int OrderId { get; set; }
    public int ReservationId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal? TotalAmount { get; set; }
}