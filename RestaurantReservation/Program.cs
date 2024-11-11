using System;
using System.Threading.Tasks;
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
            await RemoveTest();
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
