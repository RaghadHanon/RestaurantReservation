using FluentValidation;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.API.ValidationMessages;

namespace RestaurantReservation.API.Validators;
public class OrderUpdateDtoValidator : AbstractValidator<OrderUpdateDto>
{
    public OrderUpdateDtoValidator()
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
