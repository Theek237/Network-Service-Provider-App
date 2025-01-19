// Location: NetworkServiceProvider/Entities/Customer.cs

using System;
using System.Collections.Generic;

namespace NetworkServiceProvider.Entities
{
    public class Customer : IEntity
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; private set; }
        public List<ServicePlan> ServicePlans { get; private set; }
        public List<SupportTicket> SupportTickets { get; private set; }
        public List<Bill> Bills { get; private set; }

        public Customer(int id, string name, string email, string phoneNumber, string address)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            RegistrationDate = DateTime.UtcNow;
            ServicePlans = new List<ServicePlan>();
            SupportTickets = new List<SupportTicket>();
            Bills = new List<Bill>();
        }

        public void AddServicePlan(ServicePlan plan)
        {
            if (plan == null)
                throw new ArgumentNullException(nameof(plan));

            if (!ServicePlans.Contains(plan))
                ServicePlans.Add(plan);
        }

        public void RemoveServicePlan(ServicePlan plan)
        {
            ServicePlans.Remove(plan);
        }
    }
}