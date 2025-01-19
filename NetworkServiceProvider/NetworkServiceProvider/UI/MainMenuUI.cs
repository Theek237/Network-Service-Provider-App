// Location: NetworkServiceProvider/UI/MainMenuUI.cs

using NetworkServiceProvider.Entities;
using System;

namespace NetworkServiceProvider.UI
{
    public class MainMenuUI : BaseUI
    {
        private readonly CustomerUI customerUI;
        private readonly ServicePlanUI servicePlanUI;
        private readonly NetworkDeviceUI networkDeviceUI;
        private readonly BillingUI billingUI;
        private readonly SupportTicketUI supportTicketUI;

        public MainMenuUI(
        CustomerUI customerUI,
        ServicePlanUI servicePlanUI,
            NetworkDeviceUI networkDeviceUI,
        BillingUI billingUI,
            SupportTicketUI supportTicketUI)
        {
            this.customerUI = customerUI;
            this.servicePlanUI = servicePlanUI;
            this.networkDeviceUI = networkDeviceUI;
            this.billingUI = billingUI;
            this.supportTicketUI = supportTicketUI;
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                DisplayHeader("Network Service Provider - Main Menu");
                Console.WriteLine("1. Customer Management");
                Console.WriteLine("2. Service Plan Management");
                Console.WriteLine("3. Network Device Management");
                Console.WriteLine("4. Billing Management");
                Console.WriteLine("5. Support Ticket Management");
                Console.WriteLine("6. Performance Analysis");
                Console.WriteLine("0. Exit");
                Console.WriteLine();

                string choice = GetUserInput("Enter your choice");

                switch (choice)
                {
                    case "1":
                        customerUI.Show();
                        break;
                    case "2":
                        servicePlanUI.Show();
                        break;
                    case "3":
                        networkDeviceUI.Show();
                        break;
                    case "4":
                        billingUI.Show();
                        break;
                    case "5":
                        supportTicketUI.Show();
                        break;
                    case "6":
                        ShowPerformanceAnalysis();
                        break;
                    case "0":
                        Console.WriteLine("Thank you for using Network Service Provider. Goodbye!");
                        return;
                    default:
                        DisplayError("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void ShowPerformanceAnalysis()
        {
            DisplayHeader("Performance Analysis");
            // Implementation of performance analysis visualization
            // We'll implement this in the next section
        }
    }
}