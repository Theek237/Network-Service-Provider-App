// Location: NetworkServiceProvider/Services/CustomerService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using NetworkServiceProvider.DataStructures;
using NetworkServiceProvider.Entities;

namespace NetworkServiceProvider.Services
{
    public class CustomerService : IService<Customer>
    {
        private readonly CustomLinkedList<Customer> customers;
        private int nextCustomerId;

        public CustomerService()
        {
            customers = new CustomLinkedList<Customer>();
            nextCustomerId = 1;
        }

        public void Add(Customer customer)
        {
            ValidateCustomer(customer);
            customers.Add(customer);
        }

        public Customer GetById(int id)
        {
            return customers.FirstOrDefault(c => c.Id == id);
        }

        public List<Customer> GetAll()
        {
            return customers.ToList();
        }

        public void Update(Customer customer)
        {
            ValidateCustomer(customer);
            var existingCustomer = GetById(customer.Id);
            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with ID {customer.Id} not found.");

            // Remove old customer and add updated one
            customers.Remove(existingCustomer);
            customers.Add(customer);
        }

        public bool Delete(int id)
        {
            var customer = GetById(id);
            if (customer == null)
                return false;

            return customers.Remove(customer);
        }

        public Customer CreateCustomer(string name, string email, string phoneNumber, string address)
        {
            var customer = new Customer(nextCustomerId++, name, email, phoneNumber, address);
            Add(customer);
            return customer;
        }

        private void ValidateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (string.IsNullOrWhiteSpace(customer.Name))
                throw new ArgumentException("Customer name cannot be empty");

            if (string.IsNullOrWhiteSpace(customer.Email) || !customer.Email.Contains("@"))
                throw new ArgumentException("Invalid email format");

            if (string.IsNullOrWhiteSpace(customer.PhoneNumber))
                throw new ArgumentException("Phone number cannot be empty");

            if (string.IsNullOrWhiteSpace(customer.Address))
                throw new ArgumentException("Address cannot be empty");
        }
    }
}