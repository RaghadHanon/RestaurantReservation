using AutoMapper;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles;
public class MenuItemProfile : Profile
{
    public MenuItemProfile()
    {
        CreateMap<MenuItem, MenuItemDto>();
        CreateMap<MenuItem, MenuItemWithOrderItemsDto>();
        CreateMap<MenuItemCreationDto, MenuItem>();
        CreateMap<MenuItemUpdateDto, MenuItem>();
        CreateMap<MenuItem, MenuItemUpdateDto>();
    }
}
