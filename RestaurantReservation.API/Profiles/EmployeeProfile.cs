using AutoMapper;
using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles;
public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>();
        CreateMap<Employee, EmployeeWithOrdersDto>();
        CreateMap<EmployeeCreationDto, Employee>();
        CreateMap<EmployeeUpdateDto, Employee>();
        CreateMap<Employee, EmployeeUpdateDto>();
    }
}
