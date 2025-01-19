// Location: NetworkServiceProvider/UI/SupportTicketUI.cs

using System;
using System.Linq;
using NetworkServiceProvider.Services;
using NetworkServiceProvider.Entities;

namespace NetworkServiceProvider.UI
{
    public class SupportTicketUI : BaseUI
    {
        private readonly SupportTicketService supportTicketService;
        private readonly CustomerService customerService;

        public SupportTicketUI(SupportTicketService supportTicketService, CustomerService customerService)
        {
            this.supportTicketService = supportTicketService;
            this.customerService = customerService;
        }

        public void Show()
        {
            while (true)
            {
                DisplayHeader("Support Ticket Management");
                Console.WriteLine("1. View All Tickets");
                Console.WriteLine("2. Create New Ticket");
                Console.WriteLine("3. Update Ticket Status");
                Console.WriteLine("4. View Customer Tickets");
                Console.WriteLine("5. Get Next Priority Ticket");
                Console.WriteLine("6. View Ticket Statistics");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine();

                string choice = GetUserInput("Enter your choice");

                switch (choice)
                {
                    case "1":
                        ViewAllTickets();
                        break;
                    case "2":
                        CreateNewTicket();
                        break;
                    case "3":
                        UpdateTicketStatus();
                        break;
                    case "4":
                        ViewCustomerTickets();
                        break;
                    case "5":
                        GetNextPriorityTicket();
                        break;
                    case "6":
                        ViewTicketStatistics();
                        break;
                    case "0":
                        return;
                    default:
                        DisplayError("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void ViewAllTickets()
        {
            DisplayHeader("All Support Tickets");
            var tickets = supportTicketService.GetAll();

            if (!tickets.Any())
            {
                Console.WriteLine("No tickets found.");
                Console.ReadKey();
                return;
            }

            DisplayTickets(tickets);
        }

        private void CreateNewTicket()
        {
            DisplayHeader("Create New Support Ticket");
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

                string title = GetUserInput("Enter ticket title");
                string description = GetUserInput("Enter ticket description");

                Console.WriteLine("\nPriority Levels:");
                foreach (TicketPriority priority in Enum.GetValues(typeof(TicketPriority)))
                {
                    Console.WriteLine($"{(int)priority}. {priority}");
                }

                string priorityInput = GetUserInput("Enter priority level");
                if (!Enum.TryParse(priorityInput, out TicketPriority ticketPriority))
                {
                    DisplayError("Invalid priority level");
                    return;
                }

                var ticket = supportTicketService.CreateTicket(customerId, title, description, ticketPriority);
                DisplaySuccess($"Ticket created successfully with ID: {ticket.Id}");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void UpdateTicketStatus()
        {
            DisplayHeader("Update Ticket Status");
            try
            {
                string ticketIdInput = GetUserInput("Enter ticket ID");
                if (!int.TryParse(ticketIdInput, out int ticketId))
                {
                    DisplayError("Invalid ticket ID format");
                    return;
                }

                var ticket = supportTicketService.GetById(ticketId);
                if (ticket == null)
                {
                    DisplayError("Ticket not found");
                    return;
                }

                Console.WriteLine($"\nCurrent Status: {ticket.Status}");
                Console.WriteLine("\nAvailable Statuses:");
                foreach (TicketStatus status in Enum.GetValues(typeof(TicketStatus)))
                {
                    Console.WriteLine($"{(int)status}. {status}");
                }

                string statusInput = GetUserInput("Enter new status number");
                if (!Enum.TryParse(statusInput, out TicketStatus newStatus))
                {
                    DisplayError("Invalid status");
                    return;
                }

                if (newStatus == TicketStatus.Resolved)
                {
                    supportTicketService.ResolveTicket(ticketId);
                }
                else
                {
                    ticket.Status = newStatus;
                    supportTicketService.Update(ticket);
                }

                DisplaySuccess("Ticket status updated successfully");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void ViewCustomerTickets()
        {
            DisplayHeader("View Customer Tickets");
            try
            {
                string customerIdInput = GetUserInput("Enter customer ID");
                if (!int.TryParse(customerIdInput, out int customerId))
                {
                    DisplayError("Invalid customer ID format");
                    return;
                }

                var tickets = supportTicketService.GetCustomerTickets(customerId);
                if (!tickets.Any())
                {
                    Console.WriteLine("No tickets found for this customer.");
                    Console.ReadKey();
                    return;
                }

                DisplayTickets(tickets);
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void GetNextPriorityTicket()
        {
            DisplayHeader("Next Priority Ticket");
            var ticket = supportTicketService.GetNextTicket();

            if (ticket == null)
            {
                Console.WriteLine("No pending tickets in the queue.");
                Console.ReadKey();
                return;
            }

            DisplayTicket(ticket);
            Console.ReadKey();
        }

        private void ViewTicketStatistics()
        {
            DisplayHeader("Ticket Statistics");
            var tickets = supportTicketService.GetAll();

            var statusStats = tickets
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() });

            var priorityStats = tickets
                .GroupBy(t => t.Priority)
                .Select(g => new { Priority = g.Key, Count = g.Count() });

            Console.WriteLine("Status Statistics:");
            foreach (var stat in statusStats)
            {
                Console.WriteLine($"{stat.Status}: {stat.Count}");
            }

            Console.WriteLine("\nPriority Statistics:");
            foreach (var stat in priorityStats)
            {
                Console.WriteLine($"{stat.Priority}: {stat.Count}");
            }

            var resolvedTickets = tickets.Count(t => t.Status == TicketStatus.Resolved);
            var totalTickets = tickets.Count;
            var resolutionRate = totalTickets > 0 ? (double)resolvedTickets / totalTickets * 100 : 0;

            Console.WriteLine($"\nResolution Rate: {resolutionRate:F2}%");
            Console.ReadKey();
        }

        private void DisplayTickets(System.Collections.Generic.List<SupportTicket> tickets)
        {
            foreach (var ticket in tickets)
            {
                DisplayTicket(ticket);
            }
            Console.ReadKey();
        }

        private void DisplayTicket(SupportTicket ticket)
        {
            Console.WriteLine($"Ticket ID: {ticket.Id}");
            Console.WriteLine($"Customer ID: {ticket.CustomerId}");
            Console.WriteLine($"Title: {ticket.Title}");
            Console.WriteLine($"Description: {ticket.Description}");
            Console.WriteLine($"Priority: {ticket.Priority}");
            Console.WriteLine($"Status: {ticket.Status}");
            Console.WriteLine($"Created: {ticket.CreatedDate:yyyy-MM-dd HH:mm:ss}");
            if (ticket.ResolvedDate.HasValue)
            {
                Console.WriteLine($"Resolved: {ticket.ResolvedDate.Value:yyyy-MM-dd HH:mm:ss}");
            }
            Console.WriteLine(new string('-', 30));
        }
    }
}