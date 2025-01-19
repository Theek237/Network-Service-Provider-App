using System;

namespace NetworkServiceProvider.Entities
{
    public class Bill : IEntity
    {
        public int Id { get; private set; }
        public int CustomerId { get; private set; }
        public decimal Amount { get; set; }
        public DateTime BillingDate { get; set; }
        public DateTime DueDate { get; set; }
        public BillStatus Status { get; set; }
        public string Description { get; set; }

        public Bill(int id, int customerId, decimal amount, DateTime billingDate)
        {
            Id = id;
            CustomerId = customerId;
            Amount = amount;
            BillingDate = billingDate;
            DueDate = billingDate.AddDays(30);
            Status = BillStatus.Pending;
        }
    }

    public enum BillStatus
    {
        Pending,
        Paid,
        Overdue,
        Cancelled
    }
}