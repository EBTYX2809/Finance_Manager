using Finance_Manager_Backend.BuisnessLogic.Models;
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
        Seed().Wait();
    }

    private async Task Seed()
    {
        var user1 = new User("FirstTestUser@example.com", "test_salt", "test_passwordHash", 1000);
        var user2 = new User("SecondTestUser@example.com", "test_salt", "test_passwordHash", 1100);

        var eatCategory = new Category("eat", false, "eat_icon.png", "yellow");
        var homeCategory = new Category("home", false, "home_icon.png", "brown");
        var entertaimentCategory = new Category("entertaiment", false, "entertaiment_icon.png", "blue");
        var salaryCategory = new Category("salary", true, "salary_icon.png", "green");
        var medicineCategory = new Category("medicine", false, "medicine_icon.png", "red");

        // user 1
        var user1_transaction1 = new Transaction("Bread", 10.90m, new DateTime(2025, 1, 8), eatCategory, user1);
        var user1_transaction2 = new Transaction("January salary", 10000, new DateTime(2025, 1, 18), salaryCategory, user1);
        var user1_transaction3 = new Transaction("Cinema", 100.50m, new DateTime(2025, 1, 28), entertaimentCategory, user1);
        var user1_transaction4 = new Transaction("Rent for february payment", 5000, new DateTime(2025, 2, 10), homeCategory, user1);
        var user1_transaction5 = new Transaction("Shwarma", 100.10m, new DateTime(2025, 2, 15), eatCategory, user1);
        var user1_transaction6 = new Transaction("Pills for stomach", 500.50m, new DateTime(2025, 3, 3), medicineCategory, user1);
        var user1_transaction7 = new Transaction("March salary", 8000, new DateTime(2025, 3, 23), salaryCategory, user1);
        var user1_transaction8 = new Transaction("Rent for april payment", 4000, new DateTime(2025, 4, 7), homeCategory, user1);
        var user1_transaction9 = new Transaction("Ski", 2000, new DateTime(2025, 4, 21), entertaimentCategory, user1);
        var user1_transaction10 = new Transaction("Ambulance call", 750.75m, new DateTime(2025, 5, 15), medicineCategory, user1);

        // user 2
        var user2_transaction1 = new Transaction("Video game", 800, new DateTime(2025, 1, 4), entertaimentCategory, user2);
        var user2_transaction2 = new Transaction("Pizza", 300.30m, new DateTime(2025, 1, 12), eatCategory, user2);
        var user2_transaction3 = new Transaction("Invest in building house", 2000, new DateTime(2025, 1, 25), homeCategory, user2);
        var user2_transaction4 = new Transaction("February salary", 15000, new DateTime(2025, 2, 18), salaryCategory, user2);
        var user2_transaction5 = new Transaction("Medicine tax", 500, new DateTime(2025, 2, 23), medicineCategory, user2);
        var user2_transaction6 = new Transaction("New furniture", 1000.90m, new DateTime(2025, 3, 5), homeCategory, user2);
        var user2_transaction7 = new Transaction("A lot of chocolatte", 770.34m, new DateTime(2025, 3, 11), eatCategory, user2);
        var user2_transaction8 = new Transaction("March salary", 20000, new DateTime(2025, 3, 27), salaryCategory, user2);
        var user2_transaction9 = new Transaction("Concert", 2000, new DateTime(2025, 4, 29), entertaimentCategory, user2);
        var user2_transaction10 = new Transaction("Injector", 300.94m, new DateTime(2025, 5, 6), medicineCategory, user2);

        var user1_saving1 = new Saving("Phone", 20000, user1);
        var user1_saving2 = new Saving("Japan journey", 100000, user1);

        var user2_saving1 = new Saving("House", 1000000, user2);
        var user2_saving2 = new Saving("Computer", 50000, user2);


        await dbContext.Users.AddAsync(user1);
        await dbContext.Users.AddAsync(user2);
        await dbContext.SaveChangesAsync();

        await dbContext.Categories.AddRangeAsync(eatCategory, homeCategory, entertaimentCategory, salaryCategory, medicineCategory);
        await dbContext.SaveChangesAsync();

        await dbContext.Transactions.AddRangeAsync(user1_transaction1, user1_transaction2,
            user1_transaction3, user1_transaction4, user1_transaction5, user1_transaction6,
            user1_transaction7, user1_transaction8, user1_transaction9, user1_transaction10);
        await dbContext.Transactions.AddRangeAsync(user2_transaction1, user2_transaction2,
            user2_transaction3, user2_transaction4, user2_transaction5, user2_transaction6,
            user2_transaction7, user2_transaction8, user2_transaction9, user2_transaction10);
        await dbContext.SaveChangesAsync();

        await dbContext.Savings.AddRangeAsync(user1_saving1, user1_saving2);
        await dbContext.Savings.AddRangeAsync(user2_saving1, user2_saving2);
        await dbContext.SaveChangesAsync();
    }

    private async Task ClearDataBase()
    {   
        dbContext.Users.RemoveRange(dbContext.Users); // Transactions and Savings will deleting on cascade
        await dbContext.SaveChangesAsync();

        dbContext.Categories.RemoveRange(dbContext.Categories);
        await dbContext.SaveChangesAsync();
    }

    public void Dispose() 
    {
        ClearDataBase().Wait();
        dbContext?.Dispose();
    }
}
