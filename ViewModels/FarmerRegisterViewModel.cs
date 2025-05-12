using System.ComponentModel.DataAnnotations;

namespace AgriConnect.ViewModels
{


    public class FarmerRegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Location { get; set; }
    
     //[Required]
     //   [Phone]
     //   public string ContactNumber { get; set; }  // ✅ Add this line
    }
}