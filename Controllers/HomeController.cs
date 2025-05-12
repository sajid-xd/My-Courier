using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using mycourier.Models;
using System.Diagnostics;
using System.Linq;

namespace mycourier.Controllers
{
    public class HomeController : Controller
    {
        private readonly MycourierContext _context;

        public HomeController(MycourierContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users
                        .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("id", user.Id);
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("full_name", user.FullName);
                HttpContext.Session.SetString("user_type", user.UserType);

                return user.UserType switch
                {
                    "admin" => RedirectToAction("Index", "Admin"),
                    "agent" => RedirectToAction("Index", "Agent"),
                    "user" => RedirectToAction("Index", "User"),
                    _ => RedirectToAction("Index")
                };
            }

            ViewBag.Error = "Invalid username or password!";
            return View("Index");
        }

        [HttpPost]
        public IActionResult Track(string tracking_id)
        {
            var delivery = (from d in _context.Deliveries
                            join u in _context.Users on d.SenderId equals u.Id
                            join s in _context.Services on d.ServiceId equals s.Id
                            join w in _context.Weights on d.WeightId equals w.Id
                            join l in _context.Locations on d.LocationId equals l.Id
                            where d.TrackingId == tracking_id
                            select new
                            {
                                d.ReceiverName,
                                d.FromAddress,
                                d.ToAddress,
                                d.Status,
                                d.TrackingId,
                                SenderPhone = u.PhoneNumber,
                                ServiceName = s.Name,
                                WeightName = w.Name,
                                LocationName = l.Name
                            }).FirstOrDefault();

            if (delivery == null)
            {
                ViewBag.Error = "No delivery found with that tracking ID!";
                return View("Track");
            }

            ViewBag.Delivery = delivery;
            return View("Track");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            return RedirectToAction("Index"); // Redirect to login/index page
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
