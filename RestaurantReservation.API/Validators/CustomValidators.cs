using FluentValidation;
using RestaurantReservation.API.Utilities;
using RestaurantReservation.API.ValidationMessages;
using System.Text.RegularExpressions;

namespace RestaurantReservation.Api.Validators;

/// <summary>
/// Set of Extension methods for IRuleBuilder
/// </summary>
public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> ValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
         .NotNull().NotEmpty().WithMessage(ValidationErrors.RequiredField)
         .Matches(@"^[A-Za-z\s]*$").WithMessage(ValidationErrors.OnlyLetters)
         .Length(2, 20);

    }

    public static IRuleBuilderOptions<T, string> ValidPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
         .NotNull().NotEmpty().WithMessage(ValidationErrors.RequiredField)
         .Length(10)
         .Matches(new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
         .WithMessage(ValidationErrors.PhoneNumberDigitsCount);
    }

    public static IRuleBuilderOptions<T, int> ValidId<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
         .NotEmpty()
         .NotNull().WithMessage(ValidationErrors.RequiredField)
         .InclusiveBetween(1, 1_000_000_000);
    }

    public static IRuleBuilderOptions<T, DateTime> ValidOneMonthInFutureDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
    {
        return ruleBuilder
         .NotEmpty().NotNull()
         .GreaterThanOrEqualTo(DateTime.UtcNow)
         .WithMessage(ValidationErrors.FutureDate)
         .LessThanOrEqualTo(DateTime.UtcNow.AddMonths(1))
         .WithMessage(ValidationErrors.FutureOneMonthDate);
    }

    public static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationErrors.RequiredField)
            .NotNull()
            .EmailAddress().WithMessage(ValidationErrors.EmailInvalid);
    }

    public static IRuleBuilderOptions<T, string> ValidDescription<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationErrors.RequiredField)
            .NotNull()
            .Length(10, 500).WithMessage(ValidationErrors.Length);
    }

    public static IRuleBuilderOptions<T, decimal> ValidAmount<T>(this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
             .NotNull()
             .GreaterThan(0).WithMessage(ValidationErrors.GreaterThan);
    }

    public static IRuleBuilderOptions<T, decimal> ValidPrice<T>(this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
             .NotNull()
             .GreaterThan(-1).WithMessage(ValidationErrors.GreaterThan);
    }

    public static IRuleBuilderOptions<T, int> ValidQuantity<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
             .NotNull()
             .InclusiveBetween(1, 1000).WithMessage(ValidationErrors.InclusiveBetween);
    }

    /// <summary>
    /// validate the password is Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> StrongPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().NotNull()
            .WithMessage(ApiErrors.PasswordRequired)
            .MinimumLength(8)
            .WithMessage(ValidationErrors.PasswordCountCharacters)
            .Matches("[A-Z]").WithMessage(ValidationErrors.PasswordUppercaseCharacters)
            .Matches("[a-z]").WithMessage(ValidationErrors.PasswordLowercaseCharacters)
            .Matches("[0-9]").WithMessage(ValidationErrors.PasswordDigitsCharacters)
            .Matches("[^a-zA-Z0-9]").WithMessage(ValidationErrors.PasswordSpecialCharacters);
    }
}
