﻿namespace RestaurantReservation.Db.DTOs;
public class MenuItemDto
{
    public int MenuItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}