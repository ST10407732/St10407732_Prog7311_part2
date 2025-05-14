using System.Diagnostics;
using AgriConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AgriConnect.Data;

namespace AgriConnect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AgriDbContext _context;

        public HomeController(ILogger<HomeController> logger, AgriDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string role = "User";

                if (User.IsInRole("Employee"))
                    role = "Employee";
                else if (User.IsInRole("Farmer"))
                    role = "Farmer";
                else if (User.IsInRole("Admin"))
                    role = "Admin";

                ViewBag.Role = role;

                if (role == "Farmer")
                {
                    var userEmail = User.Identity.Name;

                    var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == userEmail);

                    if (user != null)
                    {
                        var farmer = await _context.FarmerProfiles
                            .Include(f => f.User)
                            .FirstOrDefaultAsync(f => f.UserId == user.Id);

                        if (farmer != null)
                        {
                            ViewBag.FarmerName = farmer.User.FullName;
                        }
                    }
                }
            }

            return View();
        }
    }
}
