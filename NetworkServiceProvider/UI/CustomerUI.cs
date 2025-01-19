using System;
using System.Linq;
using NetworkServiceProvider.Services;
using NetworkServiceProvider.Entities;

namespace NetworkServiceProvider.UI
{
    public class CustomerUI : BaseUI
    {
        private readonly CustomerService customerService;
        private readonly ServicePlanService servicePlanService;

        public CustomerUI(CustomerService customerService, ServicePlanService servicePlanService)
        {
            this.customerService = customerService;
            this.servicePlanService = servicePlanService;
        }

        public void Show()
        {
            while (true)
            {
                DisplayHeader("Customer Management");
                Console.WriteLine("1. View All Customers");
                Console.WriteLine("2. Add New Customer");
                Console.WriteLine("3. Update Customer");
                Console.WriteLine("4. Delete Customer");
                Console.WriteLine("5. Assign Service Plan");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine();

                string choice = GetUserInput("Enter your choice");

                switch (choice)
                {
                    case "1":
                        ViewAllCustomers();
                        break;
                    case "2":
                        AddNewCustomer();
                        break;
                    case "3":
                        UpdateCustomer();
                        break;
                    case "4":
                        DeleteCustomer();
                        break;
                    case "5":
                        AssignServicePlan();
                        break;
                    case "0":
                        return;
                    default:
                        DisplayError("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void ViewAllCustomers()
        {
            DisplayHeader("All Customers");
            var customers = customerService.GetAll();

            if (!customers.Any())
            {
                Console.WriteLine("No customers found.");
                Console.ReadKey();
                return;
            }

            foreach (var customer in customers)
            {
                Console.WriteLine($"ID: {customer.Id}");
                Console.WriteLine($"Name: {customer.Name}");
                Console.WriteLine($"Email: {customer.Email}");
                Console.WriteLine($"Phone: {customer.PhoneNumber}");
                Console.WriteLine($"Address: {customer.Address}");
                Console.WriteLine(new string('-', 30));
            }
            Console.ReadKey();
        }

        private void AddNewCustomer()
        {
            DisplayHeader("Add New Customer");
            try
            {
                string name = GetUserInput("Enter customer name");
                string email = GetUserInput("Enter customer email");
                string phone = GetUserInput("Enter customer phone number");
                string address = GetUserInput("Enter customer address");

                var customer = customerService.CreateCustomer(name, email, phone, address);
                DisplaySuccess($"Customer created successfully with ID: {customer.Id}");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void UpdateCustomer()
        {
            DisplayHeader("Update Customer");
            try
            {
                string idInput = GetUserInput("Enter customer ID to update");
                if (!int.TryParse(idInput, out int id))
                {
                    DisplayError("Invalid ID format");
                    return;
                }

                var customer = customerService.GetById(id);
                if (customer == null)
                {
                    DisplayError("Customer not found");
                    return;
                }

                string name = GetUserInput($"Enter new name (current: {customer.Name})");
                string email = GetUserInput($"Enter new email (current: {customer.Email})");
                string phone = GetUserInput($"Enter new phone number (current: {customer.PhoneNumber})");
                string address = GetUserInput($"Enter new address (current: {customer.Address})");

                customer.Name = !string.IsNullOrWhiteSpace(name) ? name : customer.Name;
                customer.Email = !string.IsNullOrWhiteSpace(email) ? email : customer.Email;
                customer.PhoneNumber = !string.IsNullOrWhiteSpace(phone) ? phone : customer.PhoneNumber;
                customer.Address = !string.IsNullOrWhiteSpace(address) ? address : customer.Address;

                customerService.Update(customer);
                DisplaySuccess("Customer updated successfully");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void DeleteCustomer()
        {
            DisplayHeader("Delete Customer");
            try
            {
                string idInput = GetUserInput("Enter customer ID to delete");
                if (!int.TryParse(idInput, out int id))
                {
                    DisplayError("Invalid ID format");
                    return;
                }

                if (customerService.Delete(id))
                {
                    DisplaySuccess("Customer deleted successfully");
                }
                else
                {
                    DisplayError("Customer not found");
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void AssignServicePlan()
        {
            DisplayHeader("Assign Service Plan");
            try
            {
                string customerIdInput = GetUserInput("Enter customer ID");
                if (!int.TryParse(customerIdInput, out int customerId))
                {
                    DisplayError("Invalid customer ID format");
                    return;
                }

                var customer = customerService.GetById(customerId);
                if (customer == null)
                {
                    DisplayError("Customer not found");
                    return;
                }

                var availablePlans = servicePlanService.GetAll();
                Console.WriteLine("\nAvailable Service Plans:");
                foreach (var servicePlan in availablePlans)  
                {
                    Console.WriteLine($"ID: {servicePlan.Id} - {servicePlan.Name} (${servicePlan.MonthlyFee}/month)");
                }

                string planIdInput = GetUserInput("\nEnter service plan ID to assign");
                if (!int.TryParse(planIdInput, out int planId))
                {
                    DisplayError("Invalid plan ID format");
                    return;
                }

                var selectedPlan = servicePlanService.GetById(planId);  
                if (selectedPlan == null)
                {
                    DisplayError("Service plan not found");
                    return;
                }

                customer.AddServicePlan(selectedPlan);
                customerService.Update(customer);
                DisplaySuccess("Service plan assigned successfully");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

    }
}