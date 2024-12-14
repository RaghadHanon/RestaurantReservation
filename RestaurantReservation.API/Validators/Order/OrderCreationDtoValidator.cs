using FluentValidation;
using RestaurantReservation.Api.Validators;
using RestaurantReservation.API.Models.Order;

namespace RestaurantReservation.API.Validators;

public class OrderCreationDtoValidator : AbstractValidator<OrderCreationDto>
{
    public OrderCreationDtoValidator()
    {
        RuleFor(x => x.EmployeeId).ValidId();
        RuleFor(x => x.OrderDate).ValidOneMonthInFutureDate();
        RuleFor(x => x.TotalAmount).ValidAmount();
    }
}
