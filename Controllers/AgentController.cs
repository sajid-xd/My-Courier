using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyCourier.Models;  // This should be the correct namespace for the context
using System.Linq;
using mycourier.Models;
using Microsoft.EntityFrameworkCore;
using mycourier.Models.ViewModels;
using MyCourier.Models.ViewModels;


namespace MyCourier.Controllers
{
    public class AgentController : Controller
    {
        private readonly MycourierContext _context;

        public AgentController(MycourierContext context)
        {
            _context = context;
        }


        // Agent's Dashboard (Index)
        public IActionResult Index()
        {
            var agentId = HttpContext.Session.GetInt32("id"); // Assuming session stores user ID
            if (agentId == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var agent = _context.Users.Find(agentId);
            if (agent?.UserType != "agent")
            {
                return RedirectToAction("Logout", "Account");
            }

            var model = new AgentDashboardViewModel
            {
                AgentName = agent.FullName,
                Users = _context.Users.Where(u => u.UserType == "user").ToList(),
                Services = _context.Services.ToList(),
                Weights = _context.Weights.ToList(),
                Locations = _context.Locations.ToList(),
                Deliveries = _context.Deliveries
                    .Where(d => d.AgentId == agentId)
                    .Include(d => d.Service)    // Include related Service
                    .Include(d => d.Weight)     // Include related Weight
                    .Include(d => d.Location)   // Include related Location
                    .Include(d => d.Sender)     // Include related Sender
                    .ToList()
            };

            return View(model);
        }


        // Create a New User
        [HttpPost]
        public IActionResult CreateUser(string fullName, string username, string password, string phoneNumber, string userType)
        {
            if (_context.Users.Any(u => u.PhoneNumber == phoneNumber))
            {
                ViewBag.ErrorMessage = "Phone number already exists!";
                return RedirectToAction("Index");
            }

            var hashedPassword = password;
            var newUser = new User
            {
                FullName = fullName,
                Username = username,
                Password = hashedPassword,
                PhoneNumber = phoneNumber,
                UserType = userType
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            ViewBag.SuccessMessage = "New user added successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CreateDelivery(CreateDeliveryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // return with validation errors if any
            }

            var agentId = HttpContext.Session.GetInt32("id");

            string trackingId;
            do
            {
                trackingId = new Random().Next(1000, 9999).ToString();
            }
            while (_context.Deliveries.Any(d => d.TrackingId == trackingId));

            var newDelivery = new Delivery
            {
                FromAddress = model.FromAddress,
                ToAddress = model.ToAddress,
                SenderId = model.SenderId,
                ReceiverName = model.ReceiverName,
                ServiceId = model.ServiceId,
                WeightId = model.WeightId,
                LocationId = model.LocationId,
                AgentId = agentId.Value,
                TrackingId = trackingId,
                Status = "Pending"
            };

            _context.Deliveries.Add(newDelivery);
            _context.SaveChanges();

            ViewBag.SuccessMessage = $"Delivery created with Tracking ID: {trackingId}";
            return RedirectToAction("Index");
        }
    }
}
