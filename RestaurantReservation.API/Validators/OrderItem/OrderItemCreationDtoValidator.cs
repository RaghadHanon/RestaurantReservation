using FluentValidation;
using RestaurantReservation.API.Models.OrderItem;
using RestaurantReservation.API.ValidationMessages;

public class OrderItemCreationDtoValidator : AbstractValidator<OrderItemCreationDto>
{
    public OrderItemCreationDtoValidator()
    {
        RuleFor(x => x.ItemId)
            .GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);
    }
}
