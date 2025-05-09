using System;
using System.Collections.Generic;
using mycourier.Models;

namespace MyCourier.Models
{
    public class UserDashboardViewModel
    {
        public string UserName { get; set; }
        public List<Delivery> Deliveries { get; set; }
    }
}
