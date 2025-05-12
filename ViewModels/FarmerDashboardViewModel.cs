using AgriConnect.Models;

namespace AgriConnect.ViewModels
{
    public class FarmerDashboardViewModel
    {
        public FarmerProfile FarmerProfile { get; set; }
        public List<Product> Products { get; set; }
    }
}
