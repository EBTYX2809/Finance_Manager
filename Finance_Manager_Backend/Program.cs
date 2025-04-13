using Microsoft.EntityFrameworkCore;
using Finance_Manager_Backend.DataBase;
using Serilog;
using Finance_Manager_Backend.BuisnessLogic.Services;
using Finance_Manager_Backend.Middleware;

public class Program
{    
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();

        // DI
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<DbTransactionTemplate>();

        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<GoogleAuthService>();
        builder.Services.AddScoped<TransactionsService>();
        builder.Services.AddScoped<SavingsService>();
        builder.Services.AddScoped<AnalyticsService>();
        builder.Services.AddScoped<UsersService>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Logger
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();

        /*builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Listen(System.Net.IPAddress.Any, 18090);       // http://0.0.0.0:18090
        });*/

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Middleware
        app.UseRouting();
        app.UseMiddleware<ExceptionsHandler>();
        app.UseHttpsRedirection();
        //app.UseAuthorization();
        app.MapControllers();

        app.Run();

        Console.WriteLine("It's all good man.");
    }  
}
