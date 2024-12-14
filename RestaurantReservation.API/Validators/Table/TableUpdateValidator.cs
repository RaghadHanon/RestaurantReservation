using FluentValidation;
using RestaurantReservation.API.ModelView.Table;
using RestaurantReservation.API.ValidationMessages;

public class TableUpdateDtoValidator : AbstractValidator<TableUpdateDto>
{
    public TableUpdateDtoValidator()
    {
        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);
        RuleFor(x => x.RestaurantId)
            .GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);
    }
}
