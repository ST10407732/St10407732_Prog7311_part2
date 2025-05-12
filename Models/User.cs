using Microsoft.AspNetCore.Identity;

namespace AgriConnect.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

    }
}
