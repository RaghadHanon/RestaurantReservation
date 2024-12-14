using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.API.Validators;
using RestaurantReservation.API.Validators.Customer;
using RestaurantReservation.API.Validators.Employee;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Repositories;
using Serilog;

namespace RestaurantReservation.API;
public class Program
{
    [Obsolete]
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Host.UseSerilog();

        builder.Services.AddControllers(options =>
        {
            // Ensure JSON formatting is used by default
            options.RespectBrowserAcceptHeader = true; // Respect Accept header from the client
        }).AddJsonOptions(options =>
        {
            // Customize JSON serialization if needed (optional)
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        }).AddXmlDataContractSerializerFormatters();

        builder.Services.AddFluentValidation(config =>
        {
            config.RegisterValidatorsFromAssemblyContaining<CustomerCreationDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<CustomerUpdateDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<EmployeeCreationDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<EmployeeUpdateDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<MenuItemCreationDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<MenuItemUpdateDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<OrderItemCreationDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<OrderItemUpdateDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<OrderCreationDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<OrderUpdateDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<ReservationCreationDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<ReservationUpdateDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<RestaurantCreationDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<RestaurantUpdateDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<TableCreationDtoValidator>();
            config.RegisterValidatorsFromAssemblyContaining<TableUpdateDtoValidator>();
            config.DisableDataAnnotationsValidation = true;
        });

        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.Add("timestamp", DateTime.UtcNow);
                context.ProblemDetails.Extensions.Add("correlationId", Guid.NewGuid());
            };
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<RestaurantReservationDbContext>(
    dbContextOptions => dbContextOptions.UseSqlServer(
        builder.Configuration["ConnectionStrings:SqlServerConnection"]));

        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
        builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        builder.Services.AddScoped<ITableRepository, TableRepository>();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
