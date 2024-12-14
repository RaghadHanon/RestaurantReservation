using Asp.Versioning;
using AuthenticationService;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Repositories;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace RestaurantReservation.API;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add Controllers with JSON and FluentValidation options
        builder.Services.AddControllers()
            .AddJsonOptions(opt =>
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles) // Ignore reference cycles
            .AddNewtonsoftJson(opt =>
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        builder.Services.AddControllers()
            .AddFluentValidation(v =>
            {
                v.ImplicitlyValidateChildProperties = true;
                v.ImplicitlyValidateRootCollectionElements = true;
                v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });

        // Configure DbContext
        builder.Services.AddScoped(_ =>
        new RestaurantReservationDbContext(builder.Configuration.GetConnectionString("SqlServerConnection")));

        // Register Identity and JWT Authentication
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<RestaurantReservationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateIssuerSigningKey = true,
                   ValidateLifetime = true,
                   ValidIssuer = builder.Configuration["JWTToken:Issuer"],
                   ValidAudience = builder.Configuration["JWTToken:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.ASCII.GetBytes(builder.Configuration["JWTToken:Key"]))
               };
           });

        // Add Authorization
        builder.Services.AddAuthorization();

        //// API Versioning
        //builder.Services.AddApiVersioning(setup =>
        //{
        //    setup.DefaultApiVersion = new ApiVersion(1, 0);
        //    setup.AssumeDefaultVersionWhenUnspecified = true;
        //    setup.ReportApiVersions = true;
        //}).AddMvc();

        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.Add("timestamp", DateTime.UtcNow);
                context.ProblemDetails.Extensions.Add("correlationId", Guid.NewGuid());
            };
        });

        // Register Swagger with JWT Support
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Restaurant Reservation API",
                Version = "v1"
            });

            // Include XML Comments (Optional)
            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
            setup.IncludeXmlComments(xmlCommentsFullPath);

            // Add JWT Authorization to Swagger
            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
            });
        });

        // Register Repositories and Services
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        builder.Services.AddScoped<ITableRepository, TableRepository>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        builder.Services.AddScoped<IUserValidation, UserValidation>();
        builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // Add AutoMapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();

        // Middleware Pipeline Configuration
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication(); // Add Authentication Middleware
        app.UseAuthorization();  // Add Authorization Middleware

        app.MapControllers();

        app.Run();

    }
}