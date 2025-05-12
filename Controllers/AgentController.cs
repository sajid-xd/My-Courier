using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyCourier.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyCourier.Models.ViewModels;
using mycourier.Models;

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
                    .Include(d => d.Service)
                    .Include(d => d.Weight)
                    .Include(d => d.Location)
                    .Include(d => d.Sender)
                    .ToList()
            };

            return View(model);
        }

        // GET method to show the CreateUser form
        [HttpGet("create-user")]
        public IActionResult CreateUser()
        {
            var model = new AgentCreateUserViewModel
            {
                ExistingUsers = _context.Users
                    .Where(u => u.UserType == "user")
                    .OrderByDescending(u => u.Id)
                    .ToList()
            };

            return View(model);
        }

        // POST method to handle user creation
        [HttpPost("create-usser")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(AgentCreateUserViewModel model)
        {
            if (_context.Users.Any(u => u.PhoneNumber == model.PhoneNumber))
            {
                TempData["ErrorMessage"] = "Phone number already exists!";
                model.ExistingUsers = _context.Users.Where(u => u.UserType == "user").ToList();
                return View(model);
            }

            var newUser = new User
            {
                FullName = model.FullName,
                Username = model.Username,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                UserType = model.UserType
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "New user added successfully!";
            return RedirectToAction("CreateUser");
        }

        // GET method to delete a user
        [HttpGet("delete-user/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.UserType == "user");
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found or invalid.";
                return RedirectToAction("CreateUser");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "User deleted successfully.";
            return RedirectToAction("CreateUser");
        }

        // GET method to show the form for creating a delivery
        [HttpGet]
        public IActionResult CreateDelivery()
        {
            var agentId = HttpContext.Session.GetInt32("id");
            if (agentId == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var model = new CreateDeliveryViewModel
            {
                Users = _context.Users.Where(u => u.UserType == "user").ToList(),
                Services = _context.Services.ToList(),
                Weights = _context.Weights.ToList(),
                Locations = _context.Locations.ToList(),
                Deliveries = _context.Deliveries
                    .Where(d => d.AgentId == agentId)
                    .Include(d => d.Service)
                    .Include(d => d.Weight)
                    .Include(d => d.Location)
                    .Include(d => d.Sender)
                    .ToList()
            };

            return View(model);
        }

        // POST method to handle delivery creation
        [HttpPost]
        public IActionResult CreateDelivery(CreateDeliveryViewModel model)
        {
            var agentId = HttpContext.Session.GetInt32("id");

            if (agentId == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (!ModelState.IsValid)
            {
                // Re-populate the lists because they are null when the model is posted
                model.Users = _context.Users.Where(u => u.UserType == "user").ToList();
                model.Services = _context.Services.ToList();
                model.Weights = _context.Weights.ToList();
                model.Locations = _context.Locations.ToList();

                return View("CreateDelivery", model); // explicitly return correct view
            }

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

            TempData["SuccessMessage"] = $"Delivery created with Tracking ID: {trackingId}";
            return RedirectToAction("CreateDelivery");
        }
    }
}
