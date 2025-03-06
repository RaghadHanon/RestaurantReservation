using AutoMapper;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.API.ModelView.Restaurant;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles;
public class RestaurantProfile : Profile
{
    public RestaurantProfile()
    {
        CreateMap<Restaurant, RestaurantDto>();
        CreateMap<RestaurantCreationDto, Restaurant>();
        CreateMap<RestaurantUpdateDto, Restaurant>();
        CreateMap<Restaurant, RestaurantUpdateDto>();
        CreateMap<Restaurant, RestaurantWithEmployeesDto>();
        CreateMap<Restaurant, RestaurantWithMenuItemsDto>();
    }
}
