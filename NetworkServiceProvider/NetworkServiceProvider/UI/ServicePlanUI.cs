// Location: NetworkServiceProvider/UI/ServicePlanUI.cs

using System;
using System.Linq;
using NetworkServiceProvider.Services;
using NetworkServiceProvider.Entities;

namespace NetworkServiceProvider.UI
{
    public class ServicePlanUI : BaseUI
    {
        private readonly ServicePlanService servicePlanService;

        public ServicePlanUI(ServicePlanService servicePlanService)
        {
            this.servicePlanService = servicePlanService;
        }

        public void Show()
        {
            while (true)
            {
                DisplayHeader("Service Plan Management");
                Console.WriteLine("1. View All Service Plans");
                Console.WriteLine("2. Add New Service Plan");
                Console.WriteLine("3. Update Service Plan");
                Console.WriteLine("4. Delete Service Plan");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine();

                string choice = GetUserInput("Enter your choice");

                switch (choice)
                {
                    case "1":
                        ViewAllServicePlans();
                        break;
                    case "2":
                        AddNewServicePlan();
                        break;
                    case "3":
                        UpdateServicePlan();
                        break;
                    case "4":
                        DeleteServicePlan();
                        break;
                    case "0":
                        return;
                    default:
                        DisplayError("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void ViewAllServicePlans()
        {
            DisplayHeader("All Service Plans");
            var plans = servicePlanService.GetAll();

            if (!plans.Any())
            {
                Console.WriteLine("No service plans found.");
                Console.ReadKey();
                return;
            }

            foreach (var plan in plans)
            {
                Console.WriteLine($"ID: {plan.Id}");
                Console.WriteLine($"Name: {plan.Name}");
                Console.WriteLine($"Monthly Fee: ${plan.MonthlyFee}");
                Console.WriteLine($"Data Limit: {plan.DataLimit}GB");
                Console.WriteLine($"Speed: {plan.Speed}Mbps");
                Console.WriteLine("Features:");
                foreach (var feature in plan.Features)
                {
                    Console.WriteLine($"- {feature}");
                }
                Console.WriteLine(new string('-', 30));
            }
            Console.ReadKey();
        }

        private void AddNewServicePlan()
        {
            DisplayHeader("Add New Service Plan");
            try
            {
                string name = GetUserInput("Enter plan name");

                string monthlyFeeInput = GetUserInput("Enter monthly fee (in $)");
                if (!decimal.TryParse(monthlyFeeInput, out decimal monthlyFee))
                {
                    DisplayError("Invalid monthly fee format");
                    return;
                }

                string dataLimitInput = GetUserInput("Enter data limit (in GB)");
                if (!int.TryParse(dataLimitInput, out int dataLimit))
                {
                    DisplayError("Invalid data limit format");
                    return;
                }

                string speedInput = GetUserInput("Enter speed (in Mbps)");
                if (!int.TryParse(speedInput, out int speed))
                {
                    DisplayError("Invalid speed format");
                    return;
                }

                var plan = servicePlanService.CreateServicePlan(name, monthlyFee, dataLimit, speed);

                while (true)
                {
                    string feature = GetUserInput("Enter a feature (or press Enter to finish)");
                    if (string.IsNullOrWhiteSpace(feature))
                        break;
                    plan.AddFeature(feature);
                }

                DisplaySuccess($"Service plan created successfully with ID: {plan.Id}");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void UpdateServicePlan()
        {
            DisplayHeader("Update Service Plan");
            try
            {
                string idInput = GetUserInput("Enter service plan ID to update");
                if (!int.TryParse(idInput, out int id))
                {
                    DisplayError("Invalid ID format");
                    return;
                }

                var plan = servicePlanService.GetById(id);
                if (plan == null)
                {
                    DisplayError("Service plan not found");
                    return;
                }

                string name = GetUserInput($"Enter new name (current: {plan.Name})");
                string monthlyFeeInput = GetUserInput($"Enter new monthly fee (current: ${plan.MonthlyFee})");
                string dataLimitInput = GetUserInput($"Enter new data limit (current: {plan.DataLimit}GB)");
                string speedInput = GetUserInput($"Enter new speed (current: {plan.Speed}Mbps)");

                if (!string.IsNullOrWhiteSpace(name))
                    plan.Name = name;

                if (decimal.TryParse(monthlyFeeInput, out decimal monthlyFee))
                    plan.MonthlyFee = monthlyFee;

                if (int.TryParse(dataLimitInput, out int dataLimit))
                    plan.DataLimit = dataLimit;

                if (int.TryParse(speedInput, out int speed))
                    plan.Speed = speed;

                // Update features
                Console.WriteLine("\nCurrent features:");
                foreach (var feature in plan.Features)
                {
                    Console.WriteLine($"- {feature}");
                }

                if (GetUserInput("\nWould you like to update features? (y/n)").ToLower() == "y")
                {
                    plan.Features.Clear();
                    while (true)
                    {
                        string feature = GetUserInput("Enter a feature (or press Enter to finish)");
                        if (string.IsNullOrWhiteSpace(feature))
                            break;
                        plan.AddFeature(feature);
                    }
                }

                servicePlanService.Update(plan);
                DisplaySuccess("Service plan updated successfully");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void DeleteServicePlan()
        {
            DisplayHeader("Delete Service Plan");
            try
            {
                string idInput = GetUserInput("Enter service plan ID to delete");
                if (!int.TryParse(idInput, out int id))
                {
                    DisplayError("Invalid ID format");
                    return;
                }

                if (servicePlanService.Delete(id))
                {
                    DisplaySuccess("Service plan deleted successfully");
                }
                else
                {
                    DisplayError("Service plan not found");
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }
    }
}