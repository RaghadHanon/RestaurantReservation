using FluentValidation;
using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.API.ValidationMessages;

public class OrderItemUpdateDtoValidator : AbstractValidator<OrderItemUpdateDto>
{
    public OrderItemUpdateDtoValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);
    }
}
