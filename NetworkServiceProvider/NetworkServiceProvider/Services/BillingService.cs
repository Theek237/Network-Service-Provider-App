// Location: NetworkServiceProvider/Services/BillingService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using NetworkServiceProvider.DataStructures;
using NetworkServiceProvider.Entities;

namespace NetworkServiceProvider.Services
{
    public class BillingService : IService<Bill>
    {
        private readonly CustomLinkedList<Bill> bills;
        private readonly CustomerService customerService;
        private int nextBillId;

        public BillingService(CustomerService customerService)
        {
            this.customerService = customerService;
            bills = new CustomLinkedList<Bill>();
            nextBillId = 1;
        }

        public void Add(Bill bill)
        {
            ValidateBill(bill);
            bills.Add(bill);
        }

        public Bill GetById(int id)
        {
            return bills.FirstOrDefault(b => b.Id == id);
        }

        public List<Bill> GetAll()
        {
            return bills.ToList();
        }

        public void Update(Bill bill)
        {
            ValidateBill(bill);
            var existingBill = GetById(bill.Id);
            if (existingBill == null)
                throw new KeyNotFoundException($"Bill with ID {bill.Id} not found.");

            bills.Remove(existingBill);
            bills.Add(bill);
        }

        public bool Delete(int id)
        {
            var bill = GetById(id);
            if (bill == null)
                return false;

            return bills.Remove(bill);
        }

        public Bill GenerateBill(int customerId, decimal amount, string description)
        {
            var customer = customerService.GetById(customerId);
            if (customer == null)
                throw new ArgumentException($"Customer with ID {customerId} not found.");

            var bill = new Bill(nextBillId++, customerId, amount, DateTime.UtcNow)
            {
                Description = description
            };
            Add(bill);
            return bill;
        }

        public List<Bill> GetCustomerBills(int customerId)
        {
            return bills.Where(b => b.CustomerId == customerId).ToList();
        }

        public List<Bill> GetOverdueBills()
        {
            return bills.Where(b => b.Status == BillStatus.Pending && b.DueDate < DateTime.UtcNow).ToList();
        }

        public void ProcessPayment(int billId)
        {
            var bill = GetById(billId);
            if (bill == null)
                throw new ArgumentException($"Bill with ID {billId} not found.");

            bill.Status = BillStatus.Paid;
            Update(bill);
        }

        private void ValidateBill(Bill bill)
        {
            if (bill == null)
                throw new ArgumentNullException(nameof(bill));

            if (bill.Amount < 0)
                throw new ArgumentException("Bill amount cannot be negative");

            if (customerService.GetById(bill.CustomerId) == null)
                throw new ArgumentException($"Customer with ID {bill.CustomerId} not found.");
        }
    }
}