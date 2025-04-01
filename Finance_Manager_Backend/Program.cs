using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Models;

public class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }
    public static IConfiguration Config { get; private set; }
    
    public static void Main()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        //        

        var app = CreateWebApplication(services);

        app.Run();

        //

        Console.WriteLine("It's all good man.");
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        Config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        string? connectionString = Config.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddSingleton<UserSession>();
    }
    
    private static WebApplication CreateWebApplication(IServiceCollection services)
    {
        var builder = WebApplication.CreateBuilder();

        foreach(var service in services)
        {
            builder.Services.Add(service);
        }

        var app = builder.Build();

        // Настройка middleware
        app.UseHttpsRedirection();
        //app.UseAuthorization();
        //app.MapControllers();

        return app;
    }
}
