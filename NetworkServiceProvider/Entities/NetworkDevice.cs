using System;

namespace NetworkServiceProvider.Entities
{
    public class NetworkDevice : IEntity
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public DeviceType Type { get; set; }
        public DeviceStatus Status { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public string Location { get; set; }

        public NetworkDevice(int id, string name, string ipAddress, string macAddress, DeviceType type)
        {
            Id = id;
            Name = name;
            IPAddress = ipAddress;
            MacAddress = macAddress;
            Type = type;
            Status = DeviceStatus.Active;
            LastMaintenanceDate = DateTime.UtcNow;
        }
    }

    public enum DeviceType
    {
        Router,
        Switch,
        Firewall,
        Server,
        AccessPoint
    }

    public enum DeviceStatus
    {
        Active,
        Inactive,
        Maintenance,
        Error
    }
}