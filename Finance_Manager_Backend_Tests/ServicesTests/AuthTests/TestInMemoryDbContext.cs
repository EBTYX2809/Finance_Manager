﻿using Microsoft.EntityFrameworkCore;
using Finance_Manager_Backend.DataBase;

namespace Finance_Manager_Backend_Tests.ServicesTests.AuthTests;

public static class TestInMemoryDbContext
{    
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Уникальная БД для каждого теста
            .Options;

        var context = new AppDbContext(options);

        // Очищаем данные после каждого теста
        context.Database.EnsureCreated(); // Создаем базу, если она не существует
        context.Database.EnsureDeleted(); // Очищаем БД перед каждым тестом

        return context;
    }
}
