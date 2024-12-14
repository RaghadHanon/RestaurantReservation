using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Interfaces;
public interface ICustomerRepository
{
    Task<Customer> CreateCustomerAsync(Customer customer);
    Task<bool> CustomerExistsAsync(int id);
    void DeleteCustomer(Customer customer);
    List<Customer> FindCustomersWithPartySizeGreaterThan(int partySize);
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer?> GetCustomerAsync(int id, bool includeReservations = false);
    Task<bool> SaveChangesAsync();
    Customer UpdateCustomer(Customer customer);
}