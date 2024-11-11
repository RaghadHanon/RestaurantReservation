using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db;
public class RestaurantReservationDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservationWithCustomerAndRestaurant> ReservationWithCustomerAndRestaurant { get; set; }
    public DbSet<EmployeeWithRestaurant> EmployeeWithRestaurant { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        MapDatabaseViews(modelBuilder);
        MapDatabaseFunctions(modelBuilder);
        MapRelationshipsAndForeignKeys(modelBuilder);
        SeedData(modelBuilder);
    }

    private void MapDatabaseFunctions(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDbFunction(typeof(RestaurantReservationDbContext)
                    .GetMethod(nameof(GetTotalRevenue), new[] { typeof(int) }))
                    .HasName("GetTotalRevenue");
    }

    public decimal GetTotalRevenue(int restaurantId)
        => throw new NotSupportedException("This function is for use with EF Core only and cannot be called directly.");

    private static void MapDatabaseViews(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationWithCustomerAndRestaurant>()
                            .HasNoKey()
                            .ToView(nameof(ReservationWithCustomerAndRestaurant));

        modelBuilder.Entity<EmployeeWithRestaurant>()
                    .HasNoKey()
                    .ToView(nameof(EmployeeWithRestaurant));
    }

    private static void MapRelationshipsAndForeignKeys(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
                    .HasOne(e => e.Restaurant)
                    .WithMany(rest => rest.Employees)
                    .HasForeignKey(e => e.RestaurantId)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MenuItem>()
                    .HasOne(mi => mi.Restaurant)
                    .WithMany(rest => rest.MenuItems)
                    .HasForeignKey(mi => mi.RestaurantId)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Table>()
                    .HasOne(t => t.Restaurant)
                    .WithMany(rest => rest.Tables)
                    .HasForeignKey(t => t.RestaurantId)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Reservation>()
                    .HasOne(r => r.Table)
                    .WithMany(t => t.Reservations)
                    .HasForeignKey(r => r.TableId)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Order>()
                    .HasOne(o => o.Reservation)
                    .WithMany(r => r.Orders)
                    .HasForeignKey(o => o.ReservationId)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Order>()
                    .HasOne(o => o.Employee)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(o => o.EmployeeId)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<OrderItem>()
                    .HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<OrderItem>()
                    .HasOne(oi => oi.MenuItem)
                    .WithMany(mi => mi.OrderItems)
                    .HasForeignKey(oi => oi.ItemId)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Reservation>()
                    .HasOne(r => r.Customer)
                    .WithMany(c => c.Reservations)
                    .HasForeignKey(r => r.CustomerId)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Reservation>()
                    .HasOne(r => r.Restaurant)
                    .WithMany(res => res.Reservations)
                    .HasForeignKey(r => r.RestaurantId)
                    .OnDelete(DeleteBehavior.NoAction);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        SeedCustomer(modelBuilder);
        SeedRestaurant(modelBuilder);
        SeedEmployee(modelBuilder);
        SeedTable(modelBuilder);
        SeedMenuItem(modelBuilder);
        SeedReservation(modelBuilder);
        SeedOrder(modelBuilder);
        SeedOrderItem(modelBuilder);
    }

    private static void SeedOrderItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { OrderItemId = 1, OrderId = 1, ItemId = 1, Quantity = 2 },
            new OrderItem { OrderItemId = 2, OrderId = 1, ItemId = 2, Quantity = 1 },
            new OrderItem { OrderItemId = 3, OrderId = 2, ItemId = 3, Quantity = 1 },
            new OrderItem { OrderItemId = 4, OrderId = 3, ItemId = 4, Quantity = 2 },
            new OrderItem { OrderItemId = 5, OrderId = 4, ItemId = 5, Quantity = 1 }
        );
    }

    private static void SeedOrder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = 1, ReservationId = 1, EmployeeId = 1, OrderDate = DateTime.UtcNow.AddHours(-5), TotalAmount = 25.99m },
            new Order { OrderId = 2, ReservationId = 2, EmployeeId = 2, OrderDate = DateTime.UtcNow.AddHours(-10), TotalAmount = 42.75m },
            new Order { OrderId = 3, ReservationId = 3, EmployeeId = 3, OrderDate = DateTime.UtcNow.AddHours(-8), TotalAmount = 18.50m },
            new Order { OrderId = 4, ReservationId = 4, EmployeeId = 4, OrderDate = DateTime.UtcNow.AddHours(-3), TotalAmount = 30.00m },
            new Order { OrderId = 5, ReservationId = 5, EmployeeId = 5, OrderDate = DateTime.UtcNow.AddHours(-6), TotalAmount = 15.00m }
        );
    }

    private static void SeedReservation(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>().HasData(
            new Reservation { ReservationId = 1, CustomerId = 1, RestaurantId = 1, TableId = 1, ReservationDate = DateTime.UtcNow.AddDays(1), PartySize = 5 },
            new Reservation { ReservationId = 2, CustomerId = 2, RestaurantId = 2, TableId = 3, ReservationDate = DateTime.UtcNow.AddDays(2), PartySize = 10 },
            new Reservation { ReservationId = 3, CustomerId = 3, RestaurantId = 3, TableId = 4, ReservationDate = DateTime.UtcNow.AddDays(3), PartySize = 4 },
            new Reservation { ReservationId = 4, CustomerId = 4, RestaurantId = 4, TableId = 5, ReservationDate = DateTime.UtcNow.AddDays(4), PartySize = 2 },
            new Reservation { ReservationId = 5, CustomerId = 5, RestaurantId = 5, TableId = 1, ReservationDate = DateTime.UtcNow.AddDays(5), PartySize = 15 }
        );
    }

    private static void SeedMenuItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { ItemId = 1, RestaurantId = 1, Name = "Classic Cheeseburger", Description = "A juicy beef patty topped with cheddar cheese, lettuce, tomato, and pickles.", Price = 8.99m },
            new MenuItem { ItemId = 2, RestaurantId = 1, Name = "Caesar Salad", Description = "Romaine lettuce, croutons, parmesan cheese, and Caesar dressing.", Price = 6.50m },
            new MenuItem { ItemId = 3, RestaurantId = 2, Name = "Margherita Pizza", Description = "Traditional pizza topped with fresh mozzarella, basil, and tomato sauce.", Price = 11.99m },
            new MenuItem { ItemId = 4, RestaurantId = 2, Name = "Spaghetti Carbonara", Description = "Classic Italian pasta with pancetta, egg, and parmesan cheese.", Price = 13.50m },
            new MenuItem { ItemId = 5, RestaurantId = 3, Name = "Grilled Salmon", Description = "Salmon fillet grilled to perfection, served with steamed vegetables.", Price = 16.99m }
        );
    }

    private static void SeedTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Table>().HasData(
                    new Table { TableId = 1, RestaurantId = 1, Capacity = 5 },
                    new Table { TableId = 2, RestaurantId = 1, Capacity = 10 },
                    new Table { TableId = 3, RestaurantId = 2, Capacity = 4 },
                    new Table { TableId = 4, RestaurantId = 3, Capacity = 2 },
                    new Table { TableId = 5, RestaurantId = 4, Capacity = 15 }
                );
    }

    private static void SeedEmployee(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().HasData(
                    new Employee { EmployeeId = 1, FirstName = "Alice", LastName = "Williams", Position = "Manager", RestaurantId = 1 },
                    new Employee { EmployeeId = 2, FirstName = "Bob", LastName = "Anderson", Position = "Chef", RestaurantId = 2 },
                    new Employee { EmployeeId = 3, FirstName = "Carol", LastName = "Martinez", Position = "Waiter", RestaurantId = 3 },
                    new Employee { EmployeeId = 4, FirstName = "David", LastName = "Wilson", Position = "Host", RestaurantId = 4 },
                    new Employee { EmployeeId = 5, FirstName = "Eve", LastName = "Taylor", Position = "Cleaner", RestaurantId = 5 }
                );
    }

    private static void SeedRestaurant(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>().HasData(
                    new Restaurant { RestaurantId = 1, Name = "Gourmet Place", Address = "123 Main St", PhoneNumber = "555-1234", OpeningHours = "10 AM - 10 PM" },
                    new Restaurant { RestaurantId = 2, Name = "Fine Dine", Address = "456 Elm St", PhoneNumber = "555-5678", OpeningHours = "11 AM - 11 PM" },
                    new Restaurant { RestaurantId = 3, Name = "Bistro Cafe", Address = "789 Oak St", PhoneNumber = "555-6789", OpeningHours = "9 AM - 9 PM" },
                    new Restaurant { RestaurantId = 4, Name = "Italiano", Address = "321 Pine St", PhoneNumber = "555-8765", OpeningHours = "12 PM - 12 AM" },
                    new Restaurant { RestaurantId = 5, Name = "Quick Bites", Address = "654 Maple St", PhoneNumber = "555-4321", OpeningHours = "8 AM - 8 PM" }
                );
    }

    private static void SeedCustomer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasData(
                    new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" },
                    new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PhoneNumber = "0987654321" },
                    new Customer { CustomerId = 3, FirstName = "Emily", LastName = "Clark", Email = "emily.clark@example.com", PhoneNumber = "5555555555" },
                    new Customer { CustomerId = 4, FirstName = "Michael", LastName = "Johnson", Email = "michael.johnson@example.com", PhoneNumber = "4444444444" },
                    new Customer { CustomerId = 5, FirstName = "Sarah", LastName = "Brown", Email = "sarah.brown@example.com", PhoneNumber = "3333333333" }
                );
    }
}
