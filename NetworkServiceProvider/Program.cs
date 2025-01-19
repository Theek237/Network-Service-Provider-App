using System;
using Microsoft.Extensions.DependencyInjection;
using NetworkServiceProvider.Services;
using NetworkServiceProvider.UI;

namespace NetworkServiceProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Network Service Provider - Management System";

            // Set up Dependency Injection
            var serviceProvider = ConfigureServices();

            try
            {
                // Get main menu UI and start the application
                var mainMenuUI = serviceProvider.GetRequiredService<MainMenuUI>();
                mainMenuUI.ShowMainMenu();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Fatal error: {ex.Message}");
                Console.WriteLine("Please contact system administrator.");
                Console.ResetColor();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register Services
            services.AddSingleton<CustomerService>();
            services.AddSingleton<ServicePlanService>();
            services.AddSingleton<NetworkDeviceService>();
            services.AddSingleton<BillingService>();
            services.AddSingleton<SupportTicketService>();

            // Register UI Components
            services.AddSingleton<CustomerUI>();
            services.AddSingleton<ServicePlanUI>();
            services.AddSingleton<NetworkDeviceUI>();
            services.AddSingleton<BillingUI>();
            services.AddSingleton<SupportTicketUI>();
            services.AddSingleton<MainMenuUI>();

            return services.BuildServiceProvider();
        }
    }
}