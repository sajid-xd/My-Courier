using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using mycourier.Models;
using System.Linq;

namespace mycourier.Controllers
{
    public class AdminController : Controller
    {
        private readonly MycourierContext _context;

        public AdminController(MycourierContext context)
        {
            _context = context;
        }

        // Action to show the admin dashboard
        public IActionResult Index()
        {
            // Check if the user is logged in and is an admin
            if (HttpContext.Session.GetString("user_type") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var users = _context.Users.ToList();
            var services = _context.Services.ToList();
            var locations = _context.Locations.ToList();
            var weights = _context.Weights.ToList();

            ViewBag.Users = users;
            ViewBag.Services = services;
            ViewBag.Locations = locations;
            ViewBag.Weights = weights;

            return View();
        }

        // Action to add a new user
        // GET: Admin/CreateUser or Admin/CreateUser/{id}
        // If id is passed, load user data for editing; else new user
        [HttpGet]
        public IActionResult CreateUser(int? id)
        {
            if (HttpContext.Session.GetString("user_type") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var users = _context.Users.ToList();
            ViewBag.Users = users;

            User user = null;
            if (id.HasValue)
            {
                user = _context.Users.Find(id.Value);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction("CreateUser");
                }
            }

            return View(user);  // Pass user model (null if new)
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(int? id, string fullName, string username, string password, string phoneNumber, string userType)
        {
            if (id.HasValue)
            {
                // Update existing user
                var userToUpdate = _context.Users.Find(id.Value);
                if (userToUpdate == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction("CreateUser");
                }

                // Check for username or phone uniqueness excluding current user
                if (_context.Users.Any(u => u.Username == username && u.Id != id))
                {
                    TempData["ErrorMessage"] = "Username already exists. Please choose a different one.";
                    return RedirectToAction("CreateUser", new { id });
                }

                if (_context.Users.Any(u => u.PhoneNumber == phoneNumber && u.Id != id))
                {
                    TempData["ErrorMessage"] = "Phone number already exists. Please use a different one.";
                    return RedirectToAction("CreateUser", new { id });
                }

                userToUpdate.FullName = fullName;
                userToUpdate.Username = username;
                if (!string.IsNullOrEmpty(password))
                {
                    userToUpdate.Password = password; // You may want to hash it
                }
                userToUpdate.PhoneNumber = phoneNumber;
                userToUpdate.UserType = userType;

                _context.SaveChanges();
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("CreateUser");
            }
            else
            {
                // Create new user (your existing logic)
                if (_context.Users.Any(u => u.Username == username))
                {
                    TempData["ErrorMessage"] = "Username already exists. Please choose a different one.";
                    return RedirectToAction("CreateUser");
                }
                if (_context.Users.Any(u => u.PhoneNumber == phoneNumber))
                {
                    TempData["ErrorMessage"] = "Phone number already exists. Please use a different one.";
                    return RedirectToAction("CreateUser");
                }

                var newUser = new User
                {
                    FullName = fullName,
                    Username = username,
                    Password = password, // Consider hashing this
                    PhoneNumber = phoneNumber,
                    UserType = userType
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "New user added successfully!";
                return RedirectToAction("CreateUser");
            }
        }





        // Action to add a new service
        [HttpPost]
        public IActionResult CreateService(string name, decimal fees)
        {
            var newService = new Service
            {
                Name = name,
                Fees = fees
            };

            _context.Services.Add(newService);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "New service added successfully!";
            return RedirectToAction("CreateService");
        }
        [HttpGet]
        public IActionResult CreateService()
        {
            if (HttpContext.Session.GetString("user_type") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var services = _context.Services.ToList();
            return View(services);
        }


        // Action to add a new location
        [HttpPost]
        public IActionResult CreateLocation(string name, decimal fees)
        {
            var newLocation = new Location
            {
                Name = name,
                Fees = fees
            };

            _context.Locations.Add(newLocation);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "New location added successfully!";
            return RedirectToAction("CreateLocation");
        }
        [HttpGet]
        public IActionResult CreateLocation()
        {
            if (HttpContext.Session.GetString("user_type") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var locations = _context.Locations.ToList();
            return View(locations);
        }


        // Action to add a new weight
        [HttpPost]
        public IActionResult CreateWeight(string name, decimal fees)
        {
            var newWeight = new Weight
            {
                Name = name,
                Fees = fees
            };

            _context.Weights.Add(newWeight);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "New weight added successfully!";
            return RedirectToAction("CreateWeight");
        }
        [HttpGet]
        public IActionResult CreateWeight()
        {
            if (HttpContext.Session.GetString("user_type") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var weights = _context.Weights.ToList();
            return View(weights);
        }


        // Action to delete a user
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "User deleted successfully!";
            }

            return RedirectToAction("CreateUser");
        }

        // Action to delete a service
        public IActionResult DeleteService(int id)
        {
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Service deleted successfully!";
            }

            return RedirectToAction("CreateService");
        }

        // Action to delete a location
        public IActionResult DeleteLocation(int id)
        {
            var location = _context.Locations.Find(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Location deleted successfully!";
            }

            return RedirectToAction("CreateLocation");
        }

        // Action to delete a weight
        public IActionResult DeleteWeight(int id)
        {
            var weight = _context.Weights.Find(id);
            if (weight != null)
            {
                _context.Weights.Remove(weight);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Weight deleted successfully!";
            }

            return RedirectToAction("CreateWeight");
        }

    

    }
}
