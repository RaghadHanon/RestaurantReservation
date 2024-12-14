using System.Numerics;

namespace RestaurantReservation.API.Utilities;
public static class ApiErrors
{
    public const string DataIsNull = "Data is null.";
    public const string DataIsInvalid = "Data is invalid.";
    public const string PasswordRequired = "Password is required.";
    public const string UserNameRequired = "User Name is required.";
    public const string EmailRequired = "Email is required.";
    public const string ValidEmail = "Email must be valid.";
    public const string DeletionIsInvalid = "Cannot delete the entity, some entities are attached to it";
    public const string CustomerNotFound = "Customer Not Found";
    public const string EmployeeNotFound = "Employee Not Found";
    public const string MenuItemNotFound = "MenuItem Not Found";
    public const string OrderItemNotFound = "OrderItem Not Found";
    public const string OrderNotFound = "Order Not Found";
    public const string ReservationNotFound = "Reservation Not Found";
    public const string RestaurantNotFound = "Restaurant Not Found";
    public const string TableNotFound = "Table Not Found";
    public const string ReservationInconsistentWithMenuItem = "Reservation is not consistent with MenuItem";
    public const string EmployeeInconsistentWithReservation = "Employee is not consistent with Reservation";
    public const string BigRequest = "Big request, choose at most one list to include";
    public const string UserCreationFailed = "User creation failed! Please check user details and try again.";
    public const string UserAlreadyExists = "User already exists!";
}
