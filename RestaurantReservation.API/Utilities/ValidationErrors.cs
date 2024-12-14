namespace RestaurantReservation.API.ValidationMessages;
public static class ValidationErrors
{
    public const string RequiredField = "The {PropertyName} field is required.";
    public const string OnlyLetters = "'{PropertyName}' should only contain letters";
    public const string MaxLength = "The {PropertyName} field must not exceed {MaxLength} characters.";
    public const string MinLength = "The {PropertyName} field must be at least {MinLength} characters.";
    public const string Length = "The {PropertyName} field must be between {MinLength} and {MaxLength} characters.";
    public const string GreaterThan = "The {PropertyName} field must be greater than {ComparisonValue}.";
    public const string InclusiveBetween = "The {PropertyName} field must be between {Min} and {Max}.";
    public const string InvalidFormat = "The {PropertyName} field has an invalid format.";
    public const string FutureDate = "The {PropertyName} field must be in the future.";
    public const string FutureOneMonthDate = "'{PropertyName}' should be at most 1 month from now";
    public const string EmailInvalid= "The {PropertyName} field has an invalid email format.";
    public const string PhoneNumberInvalid= "The {PropertyName} field has an invalid phone format.";
    public const string PhoneNumberDigitsCount = "PhoneNumber should contain exactly 10 numerical digits";
    public const string PasswordCountCharacters = "Password Must be at least 8 characters";
    public const string PasswordUppercaseCharacters = "Password must include UPPERCASE letters";
    public const string PasswordLowercaseCharacters = "Password must include lowercase letters";
    public const string PasswordDigitsCharacters = "Password must include digits";
    public const string PasswordSpecialCharacters = "Password must include special characters";
}
