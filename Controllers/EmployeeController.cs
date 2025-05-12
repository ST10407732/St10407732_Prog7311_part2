using AgriConnect.Data;
using AgriConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriConnect.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly AgriDbContext _context;

        public EmployeeController(AgriDbContext context)
        {
            _context = context;
        }

        // Add a new farmer profile
        public IActionResult AddFarmerProfile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFarmerProfile(FarmerProfile farmerProfile)
        {
            if (ModelState.IsValid)
            {
                _context.FarmerProfiles.Add(farmerProfile);
                _context.SaveChanges();
                return RedirectToAction("ViewAllFarmers");
            }

            return View(farmerProfile);
        }

        // View all farmers' products with filters
        public IActionResult ViewProducts(string productType, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Products.Include(p => p.FarmerProfile).AsQueryable();

            if (!string.IsNullOrEmpty(productType))
            {
                query = query.Where(p => p.Category == productType);
            }

            if (startDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= endDate.Value);
            }

            var products = query.ToList();
            return View(products);
        }
    
// View all farmers
public async Task<IActionResult> ViewFarmers()
{
    var farmers = await _context.FarmerProfiles
        .Include(f => f.User) // assuming FarmerProfile has a navigation to User
        .ToListAsync();

    return View(farmers);
}

// View products for a specific farmer
public async Task<IActionResult> ViewFarmerProducts(int farmerId)
{
    var products = await _context.Products
        .Where(p => p.FarmerProfileId == farmerId)
        .ToListAsync();

    var farmer = await _context.FarmerProfiles
        .FirstOrDefaultAsync(f => f.Id == farmerId);

    ViewBag.FarmerName = farmer?.User?.FullName ?? "Farmers";

    return View(products);
}
    }
}