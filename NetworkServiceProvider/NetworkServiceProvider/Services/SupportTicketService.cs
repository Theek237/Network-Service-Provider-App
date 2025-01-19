// Location: NetworkServiceProvider/Services/SupportTicketService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using NetworkServiceProvider.DataStructures;
using NetworkServiceProvider.Entities;

namespace NetworkServiceProvider.Services
{
    public class SupportTicketService : IService<SupportTicket>
    {
        private readonly PriorityQueue<SupportTicket> ticketQueue;
        private readonly CustomLinkedList<SupportTicket> allTickets;
        private readonly CustomerService customerService;
        private int nextTicketId;

        public SupportTicketService(CustomerService customerService)
        {
            this.customerService = customerService;
            ticketQueue = new PriorityQueue<SupportTicket>();
            allTickets = new CustomLinkedList<SupportTicket>();
            nextTicketId = 1;
        }

        public void Add(SupportTicket ticket)
        {
            ValidateTicket(ticket);
            allTickets.Add(ticket);
            if (ticket.Status == TicketStatus.Open)
            {
                ticketQueue.Enqueue(ticket);
            }
        }

        public SupportTicket GetById(int id)
        {
            return allTickets.FirstOrDefault(t => t.Id == id);
        }

        public List<SupportTicket> GetAll()
        {
            return allTickets.ToList();
        }

        public void Update(SupportTicket ticket)
        {
            ValidateTicket(ticket);
            var existingTicket = GetById(ticket.Id);
            if (existingTicket == null)
                throw new KeyNotFoundException($"Ticket with ID {ticket.Id} not found.");

            allTickets.Remove(existingTicket);
            allTickets.Add(ticket);
        }

        public bool Delete(int id)
        {
            var ticket = GetById(id);
            if (ticket == null)
                return false;

            return allTickets.Remove(ticket);
        }

        public SupportTicket CreateTicket(int customerId, string title, string description, TicketPriority priority)
        {
            var customer = customerService.GetById(customerId);
            if (customer == null)
                throw new ArgumentException($"Customer with ID {customerId} not found.");

            var ticket = new SupportTicket(nextTicketId++, customerId, title, description, priority);
            Add(ticket);
            return ticket;
        }

        public SupportTicket GetNextTicket()
        {
            if (ticketQueue.Count == 0)
                return null;

            return ticketQueue.Dequeue();
        }

        public List<SupportTicket> GetCustomerTickets(int customerId)
        {
            return allTickets.Where(t => t.CustomerId == customerId).ToList();
        }

        public void ResolveTicket(int ticketId)
        {
            var ticket = GetById(ticketId);
            if (ticket == null)
                throw new ArgumentException($"Ticket with ID {ticketId} not found.");

            ticket.Resolve();
            Update(ticket);
        }

        private void ValidateTicket(SupportTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));

            if (string.IsNullOrWhiteSpace(ticket.Title))
                throw new ArgumentException("Ticket title cannot be empty");

            if (string.IsNullOrWhiteSpace(ticket.Description))
                throw new ArgumentException("Ticket description cannot be empty");

            if (customerService.GetById(ticket.CustomerId) == null)
                throw new ArgumentException($"Customer with ID {ticket.CustomerId} not found.");
        }
    }
}