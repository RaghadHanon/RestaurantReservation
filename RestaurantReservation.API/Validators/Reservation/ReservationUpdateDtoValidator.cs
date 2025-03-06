using FluentValidation;
using RestaurantReservation.Api.Validators;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.API.ValidationMessages;

public class ReservationUpdateDtoValidator : AbstractValidator<ReservationUpdateDto>
{
    public ReservationUpdateDtoValidator()
    {
        RuleFor(x => x.CustomerId).ValidId();
        RuleFor(x => x.RestaurantId).ValidId();
        RuleFor(x => x.TableId).ValidId();
        RuleFor(x => x.PartySize)
            .NotEmpty().NotNull()
            .InclusiveBetween(1, 30);

        RuleFor(x => x.ReservationDate).ValidOneMonthInFutureDate();
    }
}
