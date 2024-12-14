using AutoMapper;
using RestaurantReservation.API.ModelView.Restaurant;
using RestaurantReservation.API.ModelView.Table;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles;
public class TableProfile : Profile
{
    public TableProfile()
    {
        CreateMap<Table, TableDto>();
        CreateMap<Table, TableWithReservationsDto>();
        CreateMap<TableCreationDto, Table>();
        CreateMap<TableUpdateDto, Table>();
    }
}
