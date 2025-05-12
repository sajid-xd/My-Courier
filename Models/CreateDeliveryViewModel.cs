using System.Collections.Generic;
using mycourier.Models;
using MyCourier.Models; // or your correct model namespace

namespace MyCourier.Models.ViewModels
{
    public class CreateDeliveryViewModel
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public int SenderId { get; set; }
        public string ReceiverName { get; set; }
        public int ServiceId { get; set; }
        public int WeightId { get; set; }
        public int LocationId { get; set; }

        public List<User> Users { get; set; } = new();
        public List<Service> Services { get; set; } = new();
        public List<Weight> Weights { get; set; } = new();
        public List<Location> Locations { get; set; } = new();

        // ✅ This is the key part
        public List<Delivery> Deliveries { get; set; } = new();
    }
}
