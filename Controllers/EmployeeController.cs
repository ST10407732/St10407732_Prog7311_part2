using AgriConnect.Data;
using AgriConnect.Models;
using AgriConnect.ViewModels;
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
        public async Task<IActionResult> ViewFarmerProducts(int farmerId, string productType, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Products
                .Where(p => p.FarmerProfileId == farmerId);

            if (!string.IsNullOrEmpty(productType))
            {
                query = query.Where(p => p.Category == productType);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= toDate.Value);
            }

            var model = new ProductFilterViewModel
            {
                ProductType = productType,
                FromDate = fromDate,
                ToDate = toDate,
                FilteredProducts = await query.ToListAsync()
            };

            var farmer = await _context.FarmerProfiles
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == farmerId);

            ViewBag.FarmerName = farmer?.User?.FullName ?? "Unknown Farmer";
            ViewBag.FarmerId = farmerId;

            return View(model);
        }

        public async Task<IActionResult> AllProducts(string productType, DateTime? fromDate, DateTime? toDate, string farmerName)
        {
            var query = _context.Products
                .Include(p => p.FarmerProfile)
                .ThenInclude(f => f.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(productType))
                query = query.Where(p => p.Category.Contains(productType));

            if (!string.IsNullOrEmpty(farmerName))
                query = query.Where(p => p.FarmerProfile.User.FullName.Contains(farmerName));

            if (fromDate.HasValue)
                query = query.Where(p => p.ProductionDate >= fromDate);

            if (toDate.HasValue)
                query = query.Where(p => p.ProductionDate <= toDate);

            var model = new ProductSearchViewModel
            {
                ProductType = productType,
                FromDate = fromDate,
                ToDate = toDate,
                FarmerName = farmerName,
                FilteredProducts = await query.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFarmer(int id)
        {
            var farmer = await _context.FarmerProfiles.Include(f => f.User)
                                                      .FirstOrDefaultAsync(f => f.Id == id);

            if (farmer == null)
            {
                return NotFound();
            }

            // Optional: Also remove the associated user if needed
            if (farmer.User != null)
            {
                _context.Users.Remove(farmer.User);
            }

            _context.FarmerProfiles.Remove(farmer);
            await _context.SaveChangesAsync();

            return RedirectToAction("AllFarmers");
        }

    }
}