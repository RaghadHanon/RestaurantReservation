using FluentValidation;
using RestaurantReservation.Api.Validators;
using RestaurantReservation.API.Models.MenuItem;
using RestaurantReservation.API.ValidationMessages;

public class MenuItemUpdateDtoValidator : AbstractValidator<MenuItemUpdateDto>
{
    public MenuItemUpdateDtoValidator()
    {
        RuleFor(x => x.Name).ValidName();
        RuleFor(x => x.Description).ValidDescription();
        RuleFor(x => x.Price).ValidPrice();
    }
}
