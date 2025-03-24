using Finance_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager.DataBase;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Saving> Savings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //string connectionString = "server=localhost;database=TestDB;user=ebtyx;password=atm28foot";

        //options.UseSqlServer(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}
