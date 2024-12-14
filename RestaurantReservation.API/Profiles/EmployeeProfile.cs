using AutoMapper;
using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles;
public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>() 
            .ForMember(dest => dest.RestaurantName, opt => opt.MapFrom(src => src.Restaurant.Name));
        CreateMap<EmployeeDto, Employee>();
        CreateMap<Employee, EmployeeWithOrdersDto>()
            .ForMember(dest => dest.RestaurantName, opt => opt.MapFrom(src => src.Restaurant.Name));
        CreateMap<EmployeeCreationDto, Employee>();
        CreateMap<EmployeeUpdateDto, Employee>();
        CreateMap<Employee, EmployeeUpdateDto>();
    }
}
