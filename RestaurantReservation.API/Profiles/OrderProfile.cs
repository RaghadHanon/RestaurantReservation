﻿using AutoMapper;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.Db.DTOs;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles;
public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderCreationDto, Order>();
        CreateMap<OrderUpdateDto, Order>();
    }
}