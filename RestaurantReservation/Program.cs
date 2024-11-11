using RestaurantReservation.Db;
using RestaurantReservation.Db.DTOs;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //await AddTest(); 
            //await UpdateTest(); 
            //await RemoveTest();
            //await TestListManangers();
            //await TestListOrdersAndMenuItems();
            //await TestListOrderedMenuItems();
            //await TestReservationWithCustomerAndRestaurantAsync();
            //await TestEmployeeWithRestaurantAsync();
            //await TestGetTotalRevenue();
            //await TestFindCustomersByPartySize();
            //await TestCalculateAverageOrderAmount();

        }
        private async static Task TestGetTotalRevenue()
        {
            RestaurantRepository restaurantRepository = new RestaurantRepository(new RestaurantReservationDbContext());
            int restaurantId = 1;
            decimal totalRevenue = await restaurantRepository.GetTotalRevenueAsync(restaurantId);
            Console.WriteLine($"Total revenue for restaurant {restaurantId}: {totalRevenue}");
        }

        private static async Task TestFindCustomersByPartySize()
        {
            RestaurantRepository restaurantRepository = new RestaurantRepository(new RestaurantReservationDbContext());
            int PartySize = 4;
            var customers = await restaurantRepository.FindCustomersByPartySizeAsync(PartySize);
            Console.WriteLine($"Customers with reservations for party size greater than {PartySize}:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.FirstName} {customer.LastName} - Email: {customer.Email} - Phone: {customer.PhoneNumber}");
            }
        }

        public static async Task TestCalculateAverageOrderAmount()
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(new Db.RestaurantReservationDbContext());
            var employeeId = 1;
            var AverageOrderAmount = await employeeRepository.CalculateAverageOrderAmount(employeeId);
            Console.WriteLine($"Average Order Amount for Employee {employeeId}: {AverageOrderAmount}");
        }

        public static async Task TestEmployeeWithRestaurantAsync()
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(new Db.RestaurantReservationDbContext());
            var employees = await employeeRepository.GetAllEmployeeWithRestaurantAsync();
            foreach (var employee in employees)
            {
                Console.WriteLine($"Restaurant: {employee.RestaurantName}, Address: {employee.RestaurantAddress}");
                Console.WriteLine($"Restaurant Phone: {employee.RestaurantPhoneNumber}, Hours: {employee.OpeningHours}");
                Console.WriteLine($"Employee: {employee.EmployeeName}, Position: {employee.Position}");
                Console.WriteLine("--------------------------------------------------");
            }
        }

        public static async Task TestReservationWithCustomerAndRestaurantAsync()
        {
            ReservationRepository reservationRepository = new ReservationRepository(new Db.RestaurantReservationDbContext());
            var reservations = await reservationRepository.GetAllReservationWithCustomerAndRestaurantAsync();
            foreach (var reservation in reservations)
            {
                Console.WriteLine($"Reservation ID: {reservation.ReservationId}");
                Console.WriteLine($"Reservation Date: {reservation.ReservationDate}");
                Console.WriteLine($"Party Size: {reservation.PartySize}");
                Console.WriteLine($"Restaurant: {reservation.RestaurantName}, Address: {reservation.RestaurantAddress}");
                Console.WriteLine($"Restaurant Phone: {reservation.RestaurantPhoneNumber}, Hours: {reservation.OpeningHours}");
                Console.WriteLine($"Customer: {reservation.CustomerName}, Email: {reservation.CustomerEmail}, Phone: {reservation.CustomerPhoneNumber}");
                Console.WriteLine("--------------------------------------------------");
            }
        }
        private static async Task TestListOrderedMenuItems()
        {
            OrderRepository orderRepository = new OrderRepository(new Db.RestaurantReservationDbContext());
            foreach (MenuItemDto menuItemDto in await orderRepository.ListOrderedMenuItems(1))
            {
                Console.WriteLine($"{menuItemDto.Name}, {menuItemDto.Price}, {menuItemDto.Quantity}, {menuItemDto.Description}");
            }
        }

        private static async Task TestListOrdersAndMenuItems()
        {
            OrderRepository orderRepository = new OrderRepository(new Db.RestaurantReservationDbContext());
            foreach (OrderWithMenuItemsDto order in await orderRepository.ListOrdersAndMenuItems(1))
            {
                Console.WriteLine($"{order.OrderId}, {order.OrderDate}, {order.TotalAmount}");
                foreach (var menuItem in order.MenuItems)
                    Console.WriteLine($"  - {menuItem.Name}, {menuItem.Price}, {menuItem.Description}, {menuItem.Quantity}");
            }
        }

        private static async Task TestListManangers()
        {
            EmployeeRepository employeeRepository = new EmployeeRepository(new Db.RestaurantReservationDbContext());
            foreach (Employee employee in await employeeRepository.ListManagers())
            {
                Console.WriteLine($"{employee.EmployeeId}, {employee.FirstName}, {employee.LastName}, {employee.RestaurantId}, {employee.Position}");
            }
        }

        private static async Task TestAddCustomer()
        {
            CustomerRepository customerRepository = new CustomerRepository(new Db.RestaurantReservationDbContext());
            await customerRepository.AddCustomerAsync(new Customer
            {
                FirstName = "Sam",
                LastName = "Smith",
                Email = "SamSmith@gmail.com",
                PhoneNumber = "0597456789"
            });
            foreach (Customer customer in await customerRepository.GetAllCustomersAsync())
            {
                Console.WriteLine($"{customer.CustomerId}, {customer.FirstName}, {customer.LastName}, {customer.Email}, {customer.PhoneNumber}");
            }
        }

        private static async Task TestUpdateCustomer()
        {
            CustomerRepository customerRepository = new CustomerRepository(new Db.RestaurantReservationDbContext());
            await customerRepository.UpdateCustomerAsync(
                customerId: 6,
                firstName: "John",
                lastName: "Mendes",
                email: "JohnMendes@gmail.com",
                phoneNumber: "0597654189"
            );
            foreach (Customer customer in await customerRepository.GetAllCustomersAsync())
            {
                Console.WriteLine($"{customer.CustomerId}, {customer.FirstName}, {customer.LastName}, {customer.Email}, {customer.PhoneNumber}");
            }
        }

        private static async Task TestRemoveCustomer()
        {
            CustomerRepository customerRepository = new CustomerRepository(new Db.RestaurantReservationDbContext());
            await customerRepository.RemoveCustomerAsync(10);
            foreach (Customer customer in await customerRepository.GetAllCustomersAsync())
            {
                Console.WriteLine($"{customer.CustomerId}, {customer.FirstName}, {customer.LastName}, {customer.Email}, {customer.PhoneNumber}");
            }
        }
    }
}
