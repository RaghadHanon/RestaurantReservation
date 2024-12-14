using FluentValidation;
using RestaurantReservation.API.ModelView.Table;
using RestaurantReservation.API.ValidationMessages;

public class TableUpdateDtoValidator : AbstractValidator<TableUpdateDto>
{
    public TableUpdateDtoValidator()
    {
        RuleFor(x => x.Capacity)
            .NotEmpty().NotNull()
            .InclusiveBetween(1, 30).WithMessage(ValidationErrors.InclusiveBetween);
    }
}
