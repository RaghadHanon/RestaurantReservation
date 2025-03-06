using FluentValidation;
using RestaurantReservation.Api.Validators;
using RestaurantReservation.API.Models.Order;
using RestaurantReservation.API.ValidationMessages;

namespace RestaurantReservation.API.Validators;
public class OrderUpdateDtoValidator : AbstractValidator<OrderUpdateDto>
{
    public OrderUpdateDtoValidator()
    {
        RuleFor(x => x.ReservationId).ValidId();
        RuleFor(x => x.EmployeeId).ValidId();
        RuleFor(x => x.TotalAmount).ValidAmount();
    }
}
