using AutoMapper;
using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Services.AuthServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Finance_Manager_Backend.DataBase;

public static class DataSeeder
{
    public static async Task SeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

        var authData = await authService.RegisterUserAsync("test@email.com", "test_password");

        var user1 = await dbContext.Users.FirstOrDefaultAsync(u => u.Id  == authData.UserDTO.Id);

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

    public static async Task SeedAdmin(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
        var config = configuration;

        await authService.RegisterAdminAsync(configuration["AdminCredentials:Email"], configuration["AdminCredentials:Password"]);
    }

    public static async Task SeedCategories(AppDbContext dbContext)
    {
        var housing = new Category
        {
            Name = "Housing and Utilities",
            IsIncome = false,
            Icon = "housing.png",
            ColorForBackground = "#4A90E2",
        };
        housing.InnerCategories = new List<Category>
        {
            new Category { Name = "Rent", IsIncome = false, Icon = "rent.png", ColorForBackground = "#6AA9F4", ParentCategory = housing },
            new Category { Name = "Utility Bills", IsIncome = false, Icon = "utilitybills.png", ColorForBackground = "#6AA9F4", ParentCategory = housing },
            new Category { Name = "Internet", IsIncome = false, Icon = "internet.png", ColorForBackground = "#6AA9F4", ParentCategory = housing },
            new Category { Name = "Home Repair", IsIncome = false, Icon = "homerepair.png", ColorForBackground = "#6AA9F4", ParentCategory = housing },
            new Category { Name = "Furniture Purchase", IsIncome = false, Icon = "furniturepurchase.png", ColorForBackground = "#6AA9F4", ParentCategory = housing },
            new Category { Name = "Appliances", IsIncome = false, Icon = "appliances.png", ColorForBackground = "#6AA9F4", ParentCategory = housing }
        };

        var groceries = new Category
        {
            Name = "Groceries and Household Goods",
            IsIncome = false,
            Icon = "groceries.png",
            ColorForBackground = "#7ED321",
        };
        groceries.InnerCategories = new List<Category>
        {
            new Category { Name = "Food", IsIncome = false, Icon = "food.png", ColorForBackground = "#9EE843", ParentCategory = groceries },
            new Category { Name = "Household Chemicals", IsIncome = false, Icon = "householdchemicals.png", ColorForBackground = "#9EE843", ParentCategory = groceries },
            new Category { Name = "Cosmetics and Hygiene", IsIncome = false, Icon = "cosmeticshygiene.png", ColorForBackground = "#9EE843", ParentCategory = groceries }
        };

        var transport = new Category
        {
            Name = "Transport and Car",
            IsIncome = false,
            Icon = "transport.png",
            ColorForBackground = "#F5A623",
        };
        transport.InnerCategories = new List<Category>
        {
            new Category { Name = "Public Transport", IsIncome = false, Icon = "publictransport.png", ColorForBackground = "#FFC04D", ParentCategory = transport },
            new Category { Name = "Taxi", IsIncome = false, Icon = "taxi.png", ColorForBackground = "#FFC04D", ParentCategory = transport },
            new Category { Name = "Fuel Purchase", IsIncome = false, Icon = "fuelpurchase.png", ColorForBackground = "#FFC04D", ParentCategory = transport },
            new Category { Name = "Parking and Fines", IsIncome = false, Icon = "parkingfines.png", ColorForBackground = "#FFC04D", ParentCategory = transport }
        };

        var health = new Category
        {
            Name = "Health and Medicine",
            IsIncome = false,
            Icon = "health.png",
            ColorForBackground = "#D0021B",
        };
        health.InnerCategories = new List<Category>
        {
            new Category { Name = "Medical Services", IsIncome = false, Icon = "medicalservices.png", ColorForBackground = "#FF4A5B", ParentCategory = health },
            new Category { Name = "Medicines and Vitamins", IsIncome = false, Icon = "medicinesvitamins.png", ColorForBackground = "#FF4A5B", ParentCategory = health },
            new Category { Name = "Gym Membership", IsIncome = false, Icon = "gymmembership.png", ColorForBackground = "#FF4A5B", ParentCategory = health },
            new Category { Name = "Massage and Physiotherapy", IsIncome = false, Icon = "massagephysiotherapy.png", ColorForBackground = "#FF4A5B", ParentCategory = health }
        };

        var cafe = new Category
        {
            Name = "Cafes and Entertainment",
            IsIncome = false,
            Icon = "cafes.png",
            ColorForBackground = "#9013FE",
        };
        cafe.InnerCategories = new List<Category>
        {
            new Category { Name = "Cafes and Restaurants", IsIncome = false, Icon = "cafesrestaurants.png", ColorForBackground = "#B569FF", ParentCategory = cafe },
            new Category { Name = "Bars and Clubs", IsIncome = false, Icon = "barsclubs.png", ColorForBackground = "#B569FF", ParentCategory = cafe },
            new Category { Name = "Cinemas and Theaters", IsIncome = false, Icon = "cinemastheaters.png", ColorForBackground = "#B569FF", ParentCategory = cafe },
            new Category { Name = "Concerts and Festivals", IsIncome = false, Icon = "concertsfestivals.png", ColorForBackground = "#B569FF", ParentCategory = cafe },
            new Category { Name = "Amusement Parks", IsIncome = false, Icon = "amusementparks.png", ColorForBackground = "#B569FF", ParentCategory = cafe },
            new Category { Name = "Museums and Exhibitions", IsIncome = false, Icon = "museumsexhibitions.png", ColorForBackground = "#B569FF", ParentCategory = cafe },
            new Category { Name = "Sport Events", IsIncome = false, Icon = "sportevents.png", ColorForBackground = "#B569FF", ParentCategory = cafe }
        };

        var hobbies = new Category
        {
            Name = "Hobbies and Recreation",
            IsIncome = false,
            Icon = "hobbies.png",
            ColorForBackground = "#50E3C2",
        };
        hobbies.InnerCategories = new List<Category>
        {
            new Category { Name = "Books", IsIncome = false, Icon = "books.png", ColorForBackground = "#7FF5DC", ParentCategory = hobbies },
            new Category { Name = "Video Games", IsIncome = false, Icon = "videogames.png", ColorForBackground = "#7FF5DC", ParentCategory = hobbies },
            new Category { Name = "Musical Instruments", IsIncome = false, Icon = "musicalinstruments.png", ColorForBackground = "#7FF5DC", ParentCategory = hobbies },
            new Category { Name = "Streams", IsIncome = false, Icon = "streams.png", ColorForBackground = "#7FF5DC", ParentCategory = hobbies },
            new Category { Name = "Fishing", IsIncome = false, Icon = "fishing.png", ColorForBackground = "#7FF5DC", ParentCategory = hobbies }
        };

        var education = new Category
        {
            Name = "Education",
            IsIncome = false,
            Icon = "education.png",
            ColorForBackground = "#BD10E0",
        };
        education.InnerCategories = new List<Category>
        {
            new Category { Name = "Courses and Trainings", IsIncome = false, Icon = "coursestrainings.png", ColorForBackground = "#DA4EF5", ParentCategory = education },
            new Category { Name = "Tutors", IsIncome = false, Icon = "tutors.png", ColorForBackground = "#DA4EF5", ParentCategory = education },
            new Category { Name = "Education Fees", IsIncome = false, Icon = "educationfees.png", ColorForBackground = "#DA4EF5", ParentCategory = education }
        };

        var clothing = new Category
        {
            Name = "Clothing and Accessories",
            IsIncome = false,
            Icon = "clothing.png",
            ColorForBackground = "#FF8A65",
        };
        clothing.InnerCategories = new List<Category>
        {
            new Category { Name = "Clothing", IsIncome = false, Icon = "clothingitem.png", ColorForBackground = "#FFA082", ParentCategory = clothing },
            new Category { Name = "Shoes", IsIncome = false, Icon = "shoes.png", ColorForBackground = "#FFA082", ParentCategory = clothing },
            new Category { Name = "Accessories", IsIncome = false, Icon = "accessories.png", ColorForBackground = "#FFA082", ParentCategory = clothing },
            new Category { Name = "Watches and Jewelry", IsIncome = false, Icon = "watchesjewelry.png", ColorForBackground = "#FFA082", ParentCategory = clothing }
        };

        var travel = new Category
        {
            Name = "Travel and Vacation",
            IsIncome = false,
            Icon = "travel.png",
            ColorForBackground = "#00BCD4",
        };
        travel.InnerCategories = new List<Category>
        {
            new Category { Name = "Tickets", IsIncome = false, Icon = "tickets.png", ColorForBackground = "#26D5EB", ParentCategory = travel },
            new Category { Name = "Hotels", IsIncome = false, Icon = "hotels.png", ColorForBackground = "#26D5EB", ParentCategory = travel },
            new Category { Name = "Excursions", IsIncome = false, Icon = "excursions.png", ColorForBackground = "#26D5EB", ParentCategory = travel },
            new Category { Name = "Souvenirs", IsIncome = false, Icon = "souvenirs.png", ColorForBackground = "#26D5EB", ParentCategory = travel },
            new Category { Name = "Tourist Equipment", IsIncome = false, Icon = "touristequipment.png", ColorForBackground = "#26D5EB", ParentCategory = travel }
        };

        var finance = new Category
        {
            Name = "Financial Operations",
            IsIncome = false,
            Icon = "finance.png",
            ColorForBackground = "#9B9B9B",
        };
        finance.InnerCategories = new List<Category>
        {
            new Category { Name = "Loans and Debts", IsIncome = false, Icon = "loansdebts.png", ColorForBackground = "#B2B2B2", ParentCategory = finance },
            new Category { Name = "Insurance", IsIncome = false, Icon = "insurance.png", ColorForBackground = "#B2B2B2", ParentCategory = finance }
        };

        var income = new Category
        {
            Name = "Income",
            IsIncome = true,
            Icon = "income.png",
            ColorForBackground = "#7ED321",
        };
        income.InnerCategories = new List<Category>
        {
            new Category { Name = "Salary", IsIncome = true, Icon = "salary.png", ColorForBackground = "#9EE843", ParentCategory = income },
            new Category { Name = "Gift", IsIncome = true, Icon = "gift.png", ColorForBackground = "#9EE843", ParentCategory = income },
            new Category { Name = "Side Income", IsIncome = true, Icon = "sideincome.png", ColorForBackground = "#9EE843", ParentCategory = income }
        };

        var other = new Category
        {
            Name = "Other",
            IsIncome = false,
            Icon = "other.png",
            ColorForBackground = "#607D8B",
        };
        other.InnerCategories = new List<Category>
        {
            new Category { Name = "Work", IsIncome = false, Icon = "work.png", ColorForBackground = "#7899A4", ParentCategory = other },
            new Category { Name = "Pets", IsIncome = false, Icon = "pets.png", ColorForBackground = "#7899A4", ParentCategory = other },
            new Category { Name = "Gifts", IsIncome = false, Icon = "gifts.png", ColorForBackground = "#7899A4", ParentCategory = other }
        };

        // Добавляем все категории
        await dbContext.Categories.AddRangeAsync(
            housing, groceries, transport, health, cafe,
            hobbies, education, clothing, travel, finance, income, other
        );

        await dbContext.SaveChangesAsync();
    }
}
