using FluentValidation;
using RestaurantReservation.API.Models.Reservation;
using RestaurantReservation.API.ValidationMessages;

public class ReservationCreationDtoValidator : AbstractValidator<ReservationCreationDto>
{
    public ReservationCreationDtoValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);
        RuleFor(x => x.RestaurantId).GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);
        RuleFor(x => x.TableId).GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);
        RuleFor(x => x.ReservationDate)
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage(ValidationErrors.FutureDate);
        RuleFor(x => x.PartySize).GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);
    }
}
