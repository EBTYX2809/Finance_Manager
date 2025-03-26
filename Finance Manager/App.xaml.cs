using Finance_Manager.DataBase;
using Microsoft.Extensions.DependencyInjection;
using Finance_Manager.ViewModels;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Finance_Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfiguration Config { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string? connectionString = Config.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<TransactionsViewModel>();
            services.AddSingleton<AnalyticsViewModel>();
            services.AddSingleton<SavingsViewModel>();
            services.AddSingleton<PlanningViewModel>();
            services.AddSingleton<SettingsViewModel>();

            services.AddSingleton<MainWindow>();
        }      
    }
}
