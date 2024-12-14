using FluentValidation;
using RestaurantReservation.API.Models.Customer;
using RestaurantReservation.API.ValidationMessages;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Validators.Customer;
public class CustomerCreationDtoValidator : AbstractValidator<CustomerCreationDto>
{
    public CustomerCreationDtoValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage(ValidationErrors.RequiredField)
            .MaximumLength(50).WithMessage(ValidationErrors.MaxLength);

        RuleFor(c => c.LastName)
            .NotEmpty().WithMessage(ValidationErrors.RequiredField)
            .MaximumLength(50).WithMessage(ValidationErrors.MaxLength);

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage(ValidationErrors.RequiredField)
            .EmailAddress().WithMessage(ValidationErrors.EmailInvalid)
            .MaximumLength(100).WithMessage(ValidationErrors.MaxLength);

        RuleFor(c => c.PhoneNumber)
            .NotEmpty().WithMessage(ValidationErrors.RequiredField)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage(ValidationErrors.PhoneNumberInvalid);
    }
}
