using System;
using System.Collections.Generic;

namespace mycourier.Models
{
    public partial class Delivery
    {
        public int Id { get; set; }

        public string FromAddress { get; set; } = null!;

        public string ToAddress { get; set; } = null!;

        public string? SenderName { get; set; } // <-- Updated to nullable

        public string ReceiverName { get; set; } = null!;

        public int SenderId { get; set; }

        public int AgentId { get; set; }

        public int ServiceId { get; set; }

        public int WeightId { get; set; }

        public int LocationId { get; set; }

        public string TrackingId { get; set; } = null!;

        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual Service Service { get; set; } = null!;
        public virtual Weight Weight { get; set; } = null!;
        public virtual Location Location { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }
}
