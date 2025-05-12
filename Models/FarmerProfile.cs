using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriConnect.Models
{
    
        public class FarmerProfile
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 👈 ensures auto-increment
        public int Id { get; set; }


        public string UserId { get; set; }  // Foreign key to the user (the Farmer)
            public string FullName { get; set; }
            public string Location { get; set; }
         //   public string ContactNumber { get; set; }

            // Navigation property to the associated User (Farmer)
            public User User { get; set; }

            // Navigation property to related products
            public ICollection<Product> Products { get; set; }
        }
    }
