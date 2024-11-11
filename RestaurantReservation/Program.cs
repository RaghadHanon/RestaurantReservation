using System;
using System.Threading.Tasks;
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

        private static async Task AddTest()
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

        private static async Task UpdateTest()
        {
            CustomerRepository customerRepository = new CustomerRepository(new Db.RestaurantReservationDbContext());

            await customerRepository.UpdateCustomerAsync(
                customerId:6,
                firstName: "John",
                lastName: "Mendes",
                email : "JohnMendes@gmail.com",
                phoneNumber : "0597654189"
            );

            foreach (Customer customer in await customerRepository.GetAllCustomersAsync())
            {
                Console.WriteLine($"{customer.CustomerId}, {customer.FirstName}, {customer.LastName}, {customer.Email}, {customer.PhoneNumber}");
            }
        }

        private static async Task RemoveTest()
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
