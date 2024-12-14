using FluentValidation;
using RestaurantReservation.Api.Contracts.Models.Authentication;
using RestaurantReservation.API.Utilities;

namespace RestaurantReservation.Api.Validators;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().NotNull()
            .WithMessage(ApiErrors.UserNameRequired);

        RuleFor(x => x.Email)
            .NotEmpty().NotNull()
            .WithMessage(ApiErrors.EmailRequired)
            .EmailAddress()
            .WithMessage(ApiErrors.ValidEmail);

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .StrongPassword();
    }
}