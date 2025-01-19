using System.Collections.Generic;

namespace NetworkServiceProvider.Entities
{
    public class ServicePlan : IEntity
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public decimal MonthlyFee { get; set; }
        public int DataLimit { get; set; }
        public int Speed { get; set; }
        public List<string> Features { get; private set; }
        public bool IsActive { get; set; }

        public ServicePlan(int id, string name, decimal monthlyFee, int dataLimit, int speed)
        {
            Id = id;
            Name = name;
            MonthlyFee = monthlyFee;
            DataLimit = dataLimit;
            Speed = speed;
            Features = new List<string>();
            IsActive = true;
        }

        public void AddFeature(string feature)
        {
            if (!string.IsNullOrWhiteSpace(feature) && !Features.Contains(feature))
            {
                Features.Add(feature);
            }
        }

        public void RemoveFeature(string feature)
        {
            Features.Remove(feature);
        }
    }
}