namespace RestaurantReservation.API.Utilities;
public static class ApiErrors
{
    public const string DataIsNull = "Data is null.";
    public const string DataIsInvalid = "Data is invalid.";
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
}
