using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Interfaces;
public interface IMenuItemRepository
{
    MenuItem CreateMenuItem(int restaurantId, MenuItem menuItem);
    void DeleteMenuItem(MenuItem menuItem);
    Task<MenuItem?> GetMenuItemAsync(int restaurantId, int menuItemId);
    Task<IEnumerable<MenuItem>> GetMenuItemsInRestaurantAsync(int restaurantId);
    Task<IEnumerable<MenuItem>> GetOrderedMenuItemsByReservationIdAsync(int reservationId);
    Task<bool> MenuItemExistsAsync(int id);
    Task<bool> SaveChangesAsync();
}