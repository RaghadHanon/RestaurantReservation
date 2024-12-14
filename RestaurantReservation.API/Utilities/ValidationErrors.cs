namespace RestaurantReservation.API.ValidationMessages;
public static class ValidationErrors
{
    public const string RequiredField = "The {PropertyName} field is required.";
    public const string MaxLength = "The {PropertyName} field must not exceed {MaxLength} characters.";
    public const string MinLength = "The {PropertyName} field must be at least {MinLength} characters.";
    public const string Length = "The {PropertyName} field must be between {MinLength} and {MaxLength} characters.";
    public const string GreaterThan = "The {PropertyName} field must be greater than {ComparisonValue}.";
    public const string InvalidFormat = "The {PropertyName} field has an invalid format.";
    public const string FutureDate = "The {PropertyName} field must be in the future.";
    public const string EmailInvalid= "The {PropertyName} field has an invalid email format.";
    public const string PhoneNumberInvalid= "The {PropertyName} field has an invalid phone format.";
}
