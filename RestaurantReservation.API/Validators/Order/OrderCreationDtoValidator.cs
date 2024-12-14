using FluentValidation;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.API.ValidationMessages;

namespace RestaurantReservation.API.Validators;

public class OrderCreationDtoValidator : AbstractValidator<OrderCreationDto>
{
    public OrderCreationDtoValidator()
    {
        RuleFor(x => x.ReservationId)
            .GreaterThan(0)
            .WithMessage(ValidationErrors.RequiredField);

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage(ValidationErrors.RequiredField);

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0)
            .WithMessage(ValidationErrors.RequiredField);
    }
}
