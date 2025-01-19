using System;
using System.Linq;
using NetworkServiceProvider.Services;
using NetworkServiceProvider.Entities;

namespace NetworkServiceProvider.UI
{
    public class NetworkDeviceUI : BaseUI
    {
        private readonly NetworkDeviceService networkDeviceService;

        public NetworkDeviceUI(NetworkDeviceService networkDeviceService)
        {
            this.networkDeviceService = networkDeviceService;
        }

        public void Show()
        {
            while (true)
            {
                DisplayHeader("Network Device Management");
                Console.WriteLine("1. View All Devices");
                Console.WriteLine("2. Add New Device");
                Console.WriteLine("3. Update Device");
                Console.WriteLine("4. Delete Device");
                Console.WriteLine("5. Connect Devices");
                Console.WriteLine("6. View Network Topology");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine();

                string choice = GetUserInput("Enter your choice");

                switch (choice)
                {
                    case "1":
                        ViewAllDevices();
                        break;
                    case "2":
                        AddNewDevice();
                        break;
                    case "3":
                        UpdateDevice();
                        break;
                    case "4":
                        DeleteDevice();
                        break;
                    case "5":
                        ConnectDevices();
                        break;
                    case "6":
                        ViewNetworkTopology();
                        break;
                    case "0":
                        return;
                    default:
                        DisplayError("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void ViewAllDevices()
        {
            DisplayHeader("All Network Devices");
            var devices = networkDeviceService.GetAll();

            if (!devices.Any())
            {
                Console.WriteLine("No devices found.");
                Console.ReadKey();
                return;
            }

            foreach (var device in devices)
            {
                Console.WriteLine($"ID: {device.Id}");
                Console.WriteLine($"Name: {device.Name}");
                Console.WriteLine($"Type: {device.Type}");
                Console.WriteLine($"IP Address: {device.IPAddress}");
                Console.WriteLine($"MAC Address: {device.MacAddress}");
                Console.WriteLine($"Status: {device.Status}");
                Console.WriteLine($"Location: {device.Location}");
                Console.WriteLine($"Last Maintenance: {device.LastMaintenanceDate:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine(new string('-', 30));
            }
            Console.ReadKey();
        }

        

        private void AddNewDevice()
        {
            DisplayHeader("Add New Network Device");
            try
            {
                string name = GetUserInput("Enter device name");
                string ipAddress = GetUserInput("Enter IP address");
                string macAddress = GetUserInput("Enter MAC address");
                string location = GetUserInput("Enter device location");

                Console.WriteLine("\nDevice Types:");
                foreach (DeviceType type in Enum.GetValues(typeof(DeviceType)))
                {
                    Console.WriteLine($"{(int)type}. {type}");
                }

                string typeInput = GetUserInput("Enter device type number");
                if (!Enum.TryParse(typeInput, out DeviceType deviceType))
                {
                    DisplayError("Invalid device type");
                    return;
                }

                var device = networkDeviceService.CreateDevice(name, ipAddress, macAddress, deviceType, location);
                DisplaySuccess($"Network device created successfully with ID: {device.Id}");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void UpdateDevice()
        {
            DisplayHeader("Update Network Device");
            try
            {
                string idInput = GetUserInput("Enter device ID to update");
                if (!int.TryParse(idInput, out int id))
                {
                    DisplayError("Invalid ID format");
                    return;
                }

                var device = networkDeviceService.GetById(id);
                if (device == null)
                {
                    DisplayError("Device not found");
                    return;
                }

                string name = GetUserInput($"Enter new name (current: {device.Name})");
                string ipAddress = GetUserInput($"Enter new IP address (current: {device.IPAddress})");
                string macAddress = GetUserInput($"Enter new MAC address (current: {device.MacAddress})");
                string location = GetUserInput($"Enter new location (current: {device.Location})");

                Console.WriteLine("\nDevice Status:");
                foreach (DeviceStatus status in Enum.GetValues(typeof(DeviceStatus)))
                {
                    Console.WriteLine($"{(int)status}. {status}");
                }
                string statusInput = GetUserInput($"Enter new status number (current: {device.Status})");

                if (!string.IsNullOrWhiteSpace(name))
                    device.Name = name;
                if (!string.IsNullOrWhiteSpace(ipAddress))
                    device.IPAddress = ipAddress;
                if (!string.IsNullOrWhiteSpace(macAddress))
                    device.MacAddress = macAddress;
                if (!string.IsNullOrWhiteSpace(location))
                    device.Location = location;
                if (Enum.TryParse(statusInput, out DeviceStatus newStatus))
                    device.Status = newStatus;

                networkDeviceService.Update(device);
                DisplaySuccess("Device updated successfully");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void DeleteDevice()
        {
            DisplayHeader("Delete Network Device");
            try
            {
                string idInput = GetUserInput("Enter device ID to delete");
                if (!int.TryParse(idInput, out int id))
                {
                    DisplayError("Invalid ID format");
                    return;
                }

                if (networkDeviceService.Delete(id))
                {
                    DisplaySuccess("Device deleted successfully");
                }
                else
                {
                    DisplayError("Device not found");
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void ConnectDevices()
        {
            DisplayHeader("Connect Network Devices");
            try
            {
                var devices = networkDeviceService.GetAll();
                Console.WriteLine("Available devices:");
                foreach (var device in devices)
                {
                    Console.WriteLine($"ID: {device.Id} - {device.Name} ({device.Type})");
                }

                string device1IdInput = GetUserInput("\nEnter first device ID");
                string device2IdInput = GetUserInput("Enter second device ID");
                string bandwidthInput = GetUserInput("Enter bandwidth (Mbps)");

                if (!int.TryParse(device1IdInput, out int device1Id) ||
                    !int.TryParse(device2IdInput, out int device2Id) ||
                    !double.TryParse(bandwidthInput, out double bandwidth))
                {
                    DisplayError("Invalid input format");
                    return;
                }

                var device1 = networkDeviceService.GetById(device1Id);
                var device2 = networkDeviceService.GetById(device2Id);

                if (device1 == null || device2 == null)
                {
                    DisplayError("One or both devices not found");
                    return;
                }

                networkDeviceService.ConnectDevices(device1, device2, bandwidth);
                DisplaySuccess("Devices connected successfully");
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void ViewNetworkTopology()
        {
            DisplayHeader("Network Topology");
            try
            {
                var devices = networkDeviceService.GetAll();
                if (!devices.Any())
                {
                    Console.WriteLine("No devices in the network.");
                    Console.ReadKey();
                    return;
                }

                foreach (var device in devices)
                {
                    Console.WriteLine($"\nDevice: {device.Name} ({device.Type})");
                    var paths = networkDeviceService.GetShortestPaths(device);

                    Console.WriteLine("Connections:");
                    foreach (var path in paths)
                    {
                        if (path.Key != device)
                        {
                            Console.WriteLine($"-> {path.Key.Name}: {path.Value:F2} ms");
                        }
                    }
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }
    }
}