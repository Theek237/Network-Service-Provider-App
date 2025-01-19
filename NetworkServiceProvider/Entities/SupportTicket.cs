using System;

namespace NetworkServiceProvider.Entities
{
    public class SupportTicket : IEntity, IComparable<SupportTicket>
    {
        public int Id { get; private set; }
        public int CustomerId { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? ResolvedDate { get; private set; }

        public SupportTicket(int id, int customerId, string title, string description, TicketPriority priority)
        {
            Id = id;
            CustomerId = customerId;
            Title = title;
            Description = description;
            Priority = priority;
            Status = TicketStatus.Open;
            CreatedDate = DateTime.UtcNow;
        }

        public void Resolve()
        {
            Status = TicketStatus.Resolved;
            ResolvedDate = DateTime.UtcNow;
        }

        public int CompareTo(SupportTicket other)
        {
            // Compare based on priority first
            int priorityComparison = Priority.CompareTo(other.Priority);
            if (priorityComparison != 0)
                return priorityComparison;

            // If same priority, compare based on creation date
            return CreatedDate.CompareTo(other.CreatedDate);
        }
    }

    public enum TicketPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum TicketStatus
    {
        Open,
        InProgress,
        Resolved,
        Closed
    }
}