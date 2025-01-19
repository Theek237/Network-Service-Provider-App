// Location: NetworkServiceProvider/UI/BillingUI.cs

using System;
using System.Linq;
using NetworkServiceProvider.Services;
using NetworkServiceProvider.Entities;

namespace NetworkServiceProvider.UI
{
    public class BillingUI : BaseUI
    {
        private readonly BillingService billingService;
        private readonly CustomerService customerService;

        public BillingUI(BillingService billingService, CustomerService customerService)
        {
            this.billingService = billingService;
            this.customerService = customerService;
        }

        public void Show()
        {
            while (true)
            {
                DisplayHeader("Billing Management");
                Console.WriteLine("1. View All Bills");
                Console.WriteLine("2. Generate New Bill");
                Console.WriteLine("3. Process Payment");
                Console.WriteLine("4. View Customer Bills");
                Console.WriteLine("5. View Overdue Bills");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine();

                string choice = GetUserInput("Enter your choice");

                switch (choice)
                {
                    case "1":
                        ViewAllBills();
                        break;
                    case "2":
                        GenerateNewBill();
                        break;
                    case "3":
                        ProcessPayment();
                        break;
                    case "4":
                        ViewCustomerBills();
                        break;
                    case "5":
                        ViewOverdueBills();
                        break;
                    case "0":
                        return;
                    default:
                        DisplayError("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void ViewAllBills()
        {
            DisplayHeader("All Bills");
            var bills = billingService.GetAll();

            if (!bills.Any())
            {
                Console.WriteLine("No bills found.");
                Console.ReadKey();
                return;
            }

            DisplayBills(bills);
        }

        private void GenerateNewBill()
        {
            DisplayHeader("Generate New Bill");
            try
            {
                // Display all customers
                var customers = customerService.GetAll();
                Console.WriteLine("Available customers:");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"ID: {customer.Id} - {customer.Name}");
                }

                string customerIdInput = GetUserInput("\nEnter customer ID");
                if (!int.TryParse(customerIdInput, out int customerId))
                {
                    DisplayError("Invalid customer ID format");
                    return;
                }

                string amountInput = GetUserInput("Enter bill amount");
                if (!decimal.TryParse(amountInput, out decimal amount))
                {
                    DisplayError("Invalid amount format");
                    return;
                }

                string description = GetUserInput("Enter bill description");

                var bill = billingService.GenerateBill(customerId, amount, description);
                DisplaySuccess($"Bill generated successfully with ID: {bill.Id}");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void ProcessPayment()
        {
            DisplayHeader("Process Payment");
            try
            {
                string billIdInput = GetUserInput("Enter bill ID");
                if (!int.TryParse(billIdInput, out int billId))
                {
                    DisplayError("Invalid bill ID format");
                    return;
                }

                billingService.ProcessPayment(billId);
                DisplaySuccess("Payment processed successfully");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void ViewCustomerBills()
        {
            DisplayHeader("View Customer Bills");
            try
            {
                string customerIdInput = GetUserInput("Enter customer ID");
                if (!int.TryParse(customerIdInput, out int customerId))
                {
                    DisplayError("Invalid customer ID format");
                    return;
                }

                var bills = billingService.GetCustomerBills(customerId);
                if (!bills.Any())
                {
                    Console.WriteLine("No bills found for this customer.");
                    Console.ReadKey();
                    return;
                }

                DisplayBills(bills);
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void ViewOverdueBills()
        {
            DisplayHeader("Overdue Bills");
            var overdueBills = billingService.GetOverdueBills();

            if (!overdueBills.Any())
            {
                Console.WriteLine("No overdue bills found.");
                Console.ReadKey();
                return;
            }

            DisplayBills(overdueBills);
        }

        private void DisplayBills(System.Collections.Generic.List<Bill> bills)
        {
            foreach (var bill in bills)
            {
                Console.WriteLine($"Bill ID: {bill.Id}");
                Console.WriteLine($"Customer ID: {bill.CustomerId}");
                Console.WriteLine($"Amount: ${bill.Amount:F2}");
                Console.WriteLine($"Status: {bill.Status}");
                Console.WriteLine($"Billing Date: {bill.BillingDate:yyyy-MM-dd}");
                Console.WriteLine($"Due Date: {bill.DueDate:yyyy-MM-dd}");
                Console.WriteLine($"Description: {bill.Description}");
                Console.WriteLine(new string('-', 30));
            }
            Console.ReadKey();
        }
    }
}