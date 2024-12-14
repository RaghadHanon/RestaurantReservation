using AutoMapper;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles;
public class ReservationProfile :Profile
{
    public ReservationProfile()
    {
        CreateMap<Reservation, ReservationDto>();
        CreateMap<Reservation, ReservationWithOrdersDto>();
        CreateMap<ReservationCreationDto, Reservation>();
        CreateMap<ReservationUpdateDto, Reservation>();
    }
}
