using AutoMapper;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles;
public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerWithReservationsDto>();
        CreateMap<Customer, CustomerDto>();
        CreateMap<CustomerCreationDto, Customer>();
        CreateMap<CustomerUpdateDto, Customer>();
        CreateMap<Customer, CustomerUpdateDto>();
    }
}
