using System.Collections.Generic;
using mycourier.Models;

namespace MyCourier.Models
{
    public class AgentDashboardViewModel
    {
        public string AgentName { get; set; }
        public List<User> Users { get; set; }
        public List<Service> Services { get; set; }
        public List<Weight> Weights { get; set; }
        public List<Location> Locations { get; set; }
        public List<Delivery> Deliveries { get; set; }
    }
}
