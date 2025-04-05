using Microsoft.EntityFrameworkCore;
using Finance_Manager_Backend.DataBase;

public class Program
{    
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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

        app.UseHttpsRedirection();
        //app.UseAuthorization();
        app.MapControllers();

        app.Run();

        Console.WriteLine("It's all good man.");
    }  
}
