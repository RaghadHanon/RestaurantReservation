using FluentValidation;
using RestaurantReservation.Api.Validators;
using RestaurantReservation.API.Models.OrderItem;

public class OrderItemCreationDtoValidator : AbstractValidator<OrderItemCreationDto>
{
    public OrderItemCreationDtoValidator()
    {
        RuleFor(x => x.ItemId).ValidId();
        RuleFor(x => x.Quantity).ValidQuantity();
    }
}
