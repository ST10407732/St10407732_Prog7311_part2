namespace AgriConnect.Models
{
    
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public DateTime ProductionDate { get; set; }

            public int FarmerProfileId { get; set; } // Foreign key to FarmerProfile
            public FarmerProfile FarmerProfile { get; set; }  // Navigation property to FarmerProfile
        }
    }
