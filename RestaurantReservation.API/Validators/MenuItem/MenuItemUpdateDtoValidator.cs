using FluentValidation;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.ValidationMessages;

public class MenuItemUpdateDtoValidator : AbstractValidator<MenuItemUpdateDto>
{
    public MenuItemUpdateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationErrors.RequiredField)
            .Length(3, 100).WithMessage(ValidationErrors.Length);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ValidationErrors.RequiredField)
            .Length(10, 500).WithMessage(ValidationErrors.Length);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);

        RuleFor(x => x.RestaurantId)
            .GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);
    }
}
