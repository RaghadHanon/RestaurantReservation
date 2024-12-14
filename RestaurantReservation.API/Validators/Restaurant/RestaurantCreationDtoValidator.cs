using FluentValidation;
using RestaurantReservation.API.ModelView.Restaurant;
using RestaurantReservation.API.ValidationMessages;

public class RestaurantCreationDtoValidator : AbstractValidator<RestaurantCreationDto>
{
    public RestaurantCreationDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage(ValidationErrors.RequiredField)
            .MaximumLength(200).WithMessage(ValidationErrors.MaxLength);

        RuleFor(r => r.Address)
            .NotEmpty().WithMessage(ValidationErrors.RequiredField)
            .MaximumLength(200).WithMessage(ValidationErrors.MaxLength);

        RuleFor(r => r.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage(ValidationErrors.InvalidFormat)
            .When(r => !string.IsNullOrEmpty(r.PhoneNumber));

        RuleFor(r => r.OpeningHours)
            .MaximumLength(50).WithMessage(ValidationErrors.MaxLength);
    }
}
