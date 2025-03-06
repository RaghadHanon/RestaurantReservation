using FluentValidation;
using RestaurantReservation.Api.Validators;
using RestaurantReservation.API.ModelView.Restaurant;
using RestaurantReservation.API.ValidationMessages;

public class RestaurantUpdateDtoValidator : AbstractValidator<RestaurantUpdateDto>
{
    public RestaurantUpdateDtoValidator()
    {
        RuleFor(x => x.Name).ValidName();
        RuleFor(x => x.PhoneNumber).ValidPhoneNumber();
        RuleFor(x => x.OpeningHours)
            .NotEmpty()
            .NotNull()
            .MaximumLength(24).WithMessage(ValidationErrors.MaxLength);

        RuleFor(x => x.Address).NotEmpty().NotNull();
    }
}
