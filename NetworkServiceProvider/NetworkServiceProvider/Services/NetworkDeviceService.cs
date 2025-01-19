// Location: NetworkServiceProvider/Services/NetworkDeviceService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using NetworkServiceProvider.DataStructures;
using NetworkServiceProvider.Entities;

namespace NetworkServiceProvider.Services
{
    public class NetworkDeviceService : IService<NetworkDevice>
    {
        private readonly CustomLinkedList<NetworkDevice> devices;
        private readonly Graph<NetworkDevice> networkTopology;
        private int nextDeviceId;

        public NetworkDeviceService()
        {
            devices = new CustomLinkedList<NetworkDevice>();
            networkTopology = new Graph<NetworkDevice>();
            nextDeviceId = 1;
        }

        public void Add(NetworkDevice device)
        {
            ValidateDevice(device);
            devices.Add(device);
            networkTopology.AddVertex(device);
        }

        public NetworkDevice GetById(int id)
        {
            return devices.FirstOrDefault(d => d.Id == id);
        }

        public List<NetworkDevice> GetAll()
        {
            return devices.ToList();
        }

        public void Update(NetworkDevice device)
        {
            ValidateDevice(device);
            var existingDevice = GetById(device.Id);
            if (existingDevice == null)
                throw new KeyNotFoundException($"Network device with ID {device.Id} not found.");

            devices.Remove(existingDevice);
            devices.Add(device);
        }

        public bool Delete(int id)
        {
            var device = GetById(id);
            if (device == null)
                return false;

            return devices.Remove(device);
        }

        public void ConnectDevices(NetworkDevice device1, NetworkDevice device2, double bandwidth)
        {
            networkTopology.AddEdge(device1, device2, 1.0 / bandwidth); // Weight is inverse of bandwidth
        }

        public Dictionary<NetworkDevice, double> GetShortestPaths(NetworkDevice sourceDevice)
        {
            return networkTopology.Dijkstra(sourceDevice);
        }

        public NetworkDevice CreateDevice(string name, string ipAddress, string macAddress, DeviceType type, string location)
        {
            var device = new NetworkDevice(nextDeviceId++, name, ipAddress, macAddress, type)
            {
                Location = location
            };
            Add(device);
            return device;
        }

        public List<NetworkDevice> GetDevicesByType(DeviceType type)
        {
            return devices.Where(d => d.Type == type).ToList();
        }

        private void ValidateDevice(NetworkDevice device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            if (string.IsNullOrWhiteSpace(device.Name))
                throw new ArgumentException("Device name cannot be empty");

            if (string.IsNullOrWhiteSpace(device.IPAddress))
                throw new ArgumentException("IP address cannot be empty");

            if (string.IsNullOrWhiteSpace(device.MacAddress))
                throw new ArgumentException("MAC address cannot be empty");
        }
    }
}