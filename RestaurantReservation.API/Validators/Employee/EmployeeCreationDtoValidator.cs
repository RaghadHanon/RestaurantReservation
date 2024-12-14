using FluentValidation;
using RestaurantReservation.API.Models.Employee;
using RestaurantReservation.API.ValidationMessages;

namespace RestaurantReservation.API.Validators.Employee;

public class EmployeeCreationDtoValidator : AbstractValidator<EmployeeCreationDto>
{
    public EmployeeCreationDtoValidator()
    {
        RuleFor(e => e.RestaurantId)
            .NotEmpty()
            .WithMessage(ValidationErrors.RequiredField);

        RuleFor(e => e.FirstName)
            .NotEmpty()
            .WithMessage(ValidationErrors.RequiredField)
            .MaximumLength(50)
            .WithMessage(ValidationErrors.MaxLength.Replace("{MaxLength}", "50"));

        RuleFor(e => e.LastName)
            .NotEmpty()
            .WithMessage(ValidationErrors.RequiredField)
            .MaximumLength(50)
            .WithMessage(ValidationErrors.MaxLength.Replace("{MaxLength}", "50"));

        RuleFor(e => e.Position)
            .NotEmpty()
            .WithMessage(ValidationErrors.RequiredField)
            .MaximumLength(50)
            .WithMessage(ValidationErrors.MaxLength.Replace("{MaxLength}", "50"));
    }
}
