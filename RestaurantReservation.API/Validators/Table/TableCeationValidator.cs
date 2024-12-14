using FluentValidation;
using RestaurantReservation.API.ModelView.Table;
using RestaurantReservation.API.ValidationMessages;

public class TableCreationDtoValidator : AbstractValidator<TableCreationDto>
{
    public TableCreationDtoValidator()
    {
        RuleFor(x => x.Capacity)
            .NotEmpty().NotNull()
            .InclusiveBetween(1, 30).WithMessage(ValidationErrors.InclusiveBetween);
    }
}
