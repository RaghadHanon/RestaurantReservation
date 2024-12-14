using AutoMapper;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.Db.DTOs;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles;
public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>(); 
        CreateMap<Order, OrderWithMenuItemsDto>()
            .ForMember(dest => dest.MenuItems, opt =>
                opt.MapFrom(src => src.OrderItems
                    .Where(oi => oi.MenuItem != null) 
                    .Select(oi => oi.MenuItem)));
        CreateMap<OrderCreationDto, Order>();
        CreateMap<OrderUpdateDto, Order>();
        CreateMap<Order, OrderUpdateDto>();
        CreateMap<Order, OrderWithMenuItemsDto>();
        CreateMap<Order, OrderWithoutOrderItemsDto>();
    }
}
