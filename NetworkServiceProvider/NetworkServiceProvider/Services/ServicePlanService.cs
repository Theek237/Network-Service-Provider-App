// Location: NetworkServiceProvider/Services/ServicePlanService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using NetworkServiceProvider.DataStructures;
using NetworkServiceProvider.Entities;

namespace NetworkServiceProvider.Services
{
    public class ServicePlanService : IService<ServicePlan>
    {
        private readonly CustomLinkedList<ServicePlan> servicePlans;
        private int nextPlanId;

        public ServicePlanService()
        {
            servicePlans = new CustomLinkedList<ServicePlan>();
            nextPlanId = 1;
        }

        public void Add(ServicePlan plan)
        {
            ValidateServicePlan(plan);
            servicePlans.Add(plan);
        }

        public ServicePlan GetById(int id)
        {
            return servicePlans.FirstOrDefault(p => p.Id == id);
        }

        public List<ServicePlan> GetAll()
        {
            return servicePlans.ToList();
        }

        public void Update(ServicePlan plan)
        {
            ValidateServicePlan(plan);
            var existingPlan = GetById(plan.Id);
            if (existingPlan == null)
                throw new KeyNotFoundException($"Service plan with ID {plan.Id} not found.");

            servicePlans.Remove(existingPlan);
            servicePlans.Add(plan);
        }

        public bool Delete(int id)
        {
            var plan = GetById(id);
            if (plan == null)
                return false;

            return servicePlans.Remove(plan);
        }

        public ServicePlan CreateServicePlan(string name, decimal monthlyFee, int dataLimit, int speed)
        {
            var plan = new ServicePlan(nextPlanId++, name, monthlyFee, dataLimit, speed);
            Add(plan);
            return plan;
        }

        private void ValidateServicePlan(ServicePlan plan)
        {
            if (plan == null)
                throw new ArgumentNullException(nameof(plan));

            if (string.IsNullOrWhiteSpace(plan.Name))
                throw new ArgumentException("Service plan name cannot be empty");

            if (plan.MonthlyFee < 0)
                throw new ArgumentException("Monthly fee cannot be negative");

            if (plan.DataLimit < 0)
                throw new ArgumentException("Data limit cannot be negative");

            if (plan.Speed < 0)
                throw new ArgumentException("Speed cannot be negative");
        }
    }
}