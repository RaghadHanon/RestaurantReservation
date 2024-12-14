using FluentValidation;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.API.ValidationMessages;

public class ReservationUpdateDtoValidator : AbstractValidator<ReservationUpdateDto>
{
    public ReservationUpdateDtoValidator()
    {
        RuleFor(x => x.ReservationDate)
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage(ValidationErrors.FutureDate);
        RuleFor(x => x.PartySize)
            .GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);
    }
}
