namespace AgriConnect.Models
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime ProductionDate { get; set; }

        public string Description { get; set; }          // New
        public decimal Price { get; set; }               // New
        public string ImageUrl { get; set; }             // New (URL to image, e.g., in wwwroot/images)

        public int FarmerProfileId { get; set; }
        public FarmerProfile FarmerProfile { get; set; }
    }
}
