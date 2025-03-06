using FluentValidation;
using RestaurantReservation.Api.Validators;
using RestaurantReservation.API.Models.Customer;

namespace RestaurantReservation.API.Validators.Customer;
public class CustomerUpdateDtoValidator : AbstractValidator<CustomerUpdateDto>
{
    public CustomerUpdateDtoValidator()
    {
        RuleFor(c => c.FirstName).ValidName();
        RuleFor(c => c.LastName).ValidName();
        RuleFor(c => c.Email).ValidEmail();
        RuleFor(c => c.PhoneNumber).ValidPhoneNumber();
    }
}
