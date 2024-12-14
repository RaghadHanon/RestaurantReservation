using FluentValidation;
using RestaurantReservation.Api.Contracts.Models.Authentication;
using RestaurantReservation.API.Utilities;

namespace RestaurantReservation.Api.Validators;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().NotNull()
            .WithMessage(ApiErrors.UserNameRequired);

        RuleFor(x => x.Password)
            .StrongPassword();
    }
}