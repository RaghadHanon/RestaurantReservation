using FluentValidation;
using RestaurantReservation.Api.Validators;

namespace RestaurantReservation.API.Validators.Employee;

public class EmployeeCreationDtoValidator : AbstractValidator<EmployeeCreationDto>
{
    public EmployeeCreationDtoValidator()
    {
        RuleFor(e => e.RestaurantId).ValidId();
        RuleFor(e => e.FirstName).ValidName();
        RuleFor(e => e.LastName).ValidName();
        RuleFor(e => e.Position).ValidName();
    }
}
