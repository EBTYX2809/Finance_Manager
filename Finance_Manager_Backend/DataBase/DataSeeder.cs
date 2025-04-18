using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Services.AuthServices;
using System.Threading.Tasks;

namespace Finance_Manager_Backend.DataBase;

public static class DataSeeder
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

        var (user1, token) = await authService.RegisterUserAsync("test@email.com", "test_password");

        // categories
        var eatCategory = new Category("eat", false, "eat_icon.png", "yellow");
        var homeCategory = new Category("home", false, "home_icon.png", "brown");
        var entertaimentCategory = new Category("entertaiment", false, "entertaiment_icon.png", "blue");
        var salaryCategory = new Category("salary", true, "salary_icon.png", "green");
        var medicineCategory = new Category("medicine", false, "medicine_icon.png", "red");

        // user 1
        var user_transaction1 = new Transaction("Bread", 10.90m, new DateTime(2025, 1, 8), eatCategory, user1);
        var user_transaction2 = new Transaction("January salary", 10000, new DateTime(2025, 1, 18), salaryCategory, user1);
        var user_transaction3 = new Transaction("Cinema", 100.50m, new DateTime(2025, 1, 28), entertaimentCategory, user1);
        var user_transaction4 = new Transaction("Rent for february payment", 5000, new DateTime(2025, 2, 10), homeCategory, user1);
        var user_transaction5 = new Transaction("Shwarma", 100.10m, new DateTime(2025, 2, 15), eatCategory, user1);
        var user_transaction6 = new Transaction("Pills for stomach", 500.50m, new DateTime(2025, 3, 3), medicineCategory, user1);
        var user_transaction7 = new Transaction("March salary", 8000, new DateTime(2025, 3, 23), salaryCategory, user1);
        var user_transaction8 = new Transaction("Rent for april payment", 4000, new DateTime(2025, 4, 7), homeCategory, user1);
        var user_transaction9 = new Transaction("Ski", 2000, new DateTime(2025, 4, 21), entertaimentCategory, user1);
        var user_transaction10 = new Transaction("Ambulance call", 750.75m, new DateTime(2025, 5, 15), medicineCategory, user1);        
        var user_transaction11 = new Transaction("Video game", 800, new DateTime(2025, 1, 4), entertaimentCategory, user1);
        var user_transaction12 = new Transaction("Pizza", 300.30m, new DateTime(2025, 1, 12), eatCategory, user1);
        var user_transaction13 = new Transaction("Invest in building house", 2000, new DateTime(2025, 1, 25), homeCategory, user1);
        var user_transaction14 = new Transaction("February salary", 15000, new DateTime(2025, 2, 18), salaryCategory, user1);
        var user_transaction15 = new Transaction("Medicine tax", 500, new DateTime(2025, 2, 23), medicineCategory, user1);
        var user_transaction16 = new Transaction("New furniture", 1000.90m, new DateTime(2025, 3, 5), homeCategory, user1);
        var user_transaction17 = new Transaction("A lot of chocolatte", 770.34m, new DateTime(2025, 3, 11), eatCategory, user1);
        var user_transaction18 = new Transaction("March salary", 20000, new DateTime(2025, 3, 27), salaryCategory, user1);
        var user_transaction19 = new Transaction("Concert", 2000, new DateTime(2025, 4, 29), entertaimentCategory, user1);
        var user_transaction20 = new Transaction("Injector", 300.94m, new DateTime(2025, 5, 6), medicineCategory, user1);        

        // savings
        var user_saving1 = new Saving("Phone", 20000, user1);
        var user_saving2 = new Saving("Japan journey", 100000, user1);
        var user_saving3 = new Saving("House", 1000000, user1);
        var user_saving4 = new Saving("Computer", 50000, user1);

        await dbContext.Categories.AddRangeAsync(eatCategory, homeCategory, entertaimentCategory, salaryCategory, medicineCategory);
        await dbContext.SaveChangesAsync();

        await dbContext.Transactions.AddRangeAsync(user_transaction1, user_transaction2, user_transaction3, 
            user_transaction4, user_transaction5, user_transaction6, user_transaction7, user_transaction8, 
            user_transaction9, user_transaction10, user_transaction11, user_transaction12, user_transaction13, 
            user_transaction14, user_transaction15, user_transaction16, user_transaction17, user_transaction18, 
            user_transaction19, user_transaction20);
        await dbContext.SaveChangesAsync();

        await dbContext.Savings.AddRangeAsync(user_saving1, user_saving2, user_saving3, user_saving4);
        await dbContext.SaveChangesAsync();
    }

    public static async Task ClearDb(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Users.RemoveRange(dbContext.Users); // Transactions and Savings will deleting on cascade
        await dbContext.SaveChangesAsync();

        dbContext.Categories.RemoveRange(dbContext.Categories);
        await dbContext.SaveChangesAsync();
    }
}
