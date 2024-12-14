using FluentValidation;
using RestaurantReservation.Api.Validators;
using RestaurantReservation.API.Models.OrderItem;

public class OrderItemUpdateDtoValidator : AbstractValidator<OrderItemUpdateDto>
{
    public OrderItemUpdateDtoValidator()
    {
        RuleFor(x => x.Quantity).ValidQuantity();
    }
}
