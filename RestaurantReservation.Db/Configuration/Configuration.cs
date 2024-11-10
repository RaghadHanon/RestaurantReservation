using Microsoft.Extensions.Configuration;

namespace RestaurantReservation;
public static class Configuration
{
    public static IConfigurationRoot Config { get; }

    static Configuration()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        Config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) 
            .AddJsonFile(path, optional: false, reloadOnChange: true)
            .Build();
    }


    public static string GetConnectionString(string name = "RestaurantReservationConnection")
    {
        return Config.GetConnectionString(name);
    }
}

