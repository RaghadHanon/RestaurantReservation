using FluentValidation;
using RestaurantReservation.Api.Validators;
using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.API.ValidationMessages;

namespace RestaurantReservation.API.Validators.Employee;

public class EmployeeUpdateDtoValidator : AbstractValidator<EmployeeUpdateDto>
{
    public EmployeeUpdateDtoValidator()
    {
        RuleFor(e => e.RestaurantId).ValidId();
        RuleFor(e => e.FirstName).ValidName();
        RuleFor(e => e.LastName).ValidName();
        RuleFor(e => e.Position).ValidName();
    }
}
