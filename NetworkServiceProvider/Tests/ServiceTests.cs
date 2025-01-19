// Location: NetworkServiceProvider/Tests/ServiceTests.cs

using System;
using System.Linq;
using NetworkServiceProvider.Services;
using NetworkServiceProvider.Entities;
using Xunit;

namespace NetworkServiceProvider.Tests
{
    public class ServiceTests
    {
        private readonly CustomerService customerService;
        private readonly ServicePlanService servicePlanService;
        private readonly NetworkDeviceService networkDeviceService;

        public ServiceTests()
        {
            customerService = new CustomerService();
            servicePlanService = new ServicePlanService();
            networkDeviceService = new NetworkDeviceService();
        }

        [Fact]
        public void CustomerService_CreateCustomer_ShouldCreateValidCustomer()
        {
            // Arrange
            string name = "John Doe";
            string email = "john@example.com";
            string phone = "123-456-7890";
            string address = "123 Main St";

            // Act
            var customer = customerService.CreateCustomer(name, email, phone, address);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(name, customer.Name);
            Assert.Equal(email, customer.Email);
            Assert.Equal(phone, customer.PhoneNumber);
            Assert.Equal(address, customer.Address);
        }

        [Fact]
        public void ServicePlan_CreatePlan_ShouldCreateValidPlan()
        {
            // Arrange
            string name = "Premium Plan";
            decimal monthlyFee = 99.99m;
            int dataLimit = 1000;
            int speed = 100;

            // Act
            var plan = servicePlanService.CreateServicePlan(name, monthlyFee, dataLimit, speed);

            // Assert
            Assert.NotNull(plan);
            Assert.Equal(name, plan.Name);
            Assert.Equal(monthlyFee, plan.MonthlyFee);
            Assert.Equal(dataLimit, plan.DataLimit);
            Assert.Equal(speed, plan.Speed);
        }

        [Fact]
        public void NetworkDevice_ConnectDevices_ShouldCreateValidConnection()
        {
            // Arrange
            var device1 = networkDeviceService.CreateDevice("Router1", "192.168.1.1", "AA:BB:CC:DD:EE:FF", DeviceType.Router, "Main Office");
            var device2 = networkDeviceService.CreateDevice("Switch1", "192.168.1.2", "FF:EE:DD:CC:BB:AA", DeviceType.Switch, "Main Office");

            // Act
            networkDeviceService.ConnectDevices(device1, device2, 1000);
            var paths = networkDeviceService.GetShortestPaths(device1);

            // Assert
            Assert.NotNull(paths);
            Assert.Contains(device2, paths.Keys);
        }
    }
}