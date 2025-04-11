using Finance_Manager_Backend.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Finance_Manager_Backend_Tests.DataBase;

public class TestDbContextFixture : IDisposable
{
    public AppDbContext dbContext;
    public TestDbContextFixture() 
    {
        var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.Test.json")
        .Build();            

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(config.GetConnectionString("DefaultConnection"))
            .Options;

        dbContext = new AppDbContext(options);
    }

    public void Dispose() 
    {
        dbContext?.Dispose();
    }
}
