using FluentValidation;
using RestaurantReservation.Api.Validators;
using RestaurantReservation.API.Models.Customer;

namespace RestaurantReservation.API.Validators.Customer;
public class CustomerCreationDtoValidator : AbstractValidator<CustomerCreationDto>
{
    public CustomerCreationDtoValidator()
    {
        RuleFor(c => c.FirstName).ValidName();
        RuleFor(c => c.LastName).ValidName();
        RuleFor(c => c.Email).ValidEmail();
        RuleFor(c => c.PhoneNumber).ValidPhoneNumber();
    }
}
