using System.Linq;
using System.Web.Mvc;
using MyCourier.Models;
using MyCourier.Data; // Assuming your DbContext is in this namespace

namespace MyCourier.Controllers
{
    [Authorize(Roles = "User")] // Only allow authenticated users with 'User' role
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: User/Dashboard
        public ActionResult Dashboard()
        {
            // Assuming you store the logged-in user's ID in User.Identity.Name
            var username = User.Identity.Name;

            // Get the user record
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                // If the user isn't found, redirect to login
                return RedirectToAction("Login", "Home");
            }

            // Get the user's deliveries
            var deliveries = _context.Deliveries
                .Where(d => d.UserId == user.Id)
                .Select(d => new Delivery
                {
                    TrackingId = d.TrackingId,
                    ReceiverName = d.ReceiverName,
                    Service = d.Service,
                    Weight = d.Weight,
                    Location = d.Location,
                    Status = d.Status
                })
                .ToList();

            // Build the dashboard view model
            var viewModel = new UserDashboardViewModel
            {
                UserName = user.UserName,
                Deliveries = deliveries
            };

            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
