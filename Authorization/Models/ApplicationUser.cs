using Microsoft.AspNetCore.Identity;

namespace Authorization.Models
{
    public class ApplicationUser : IdentityUser 
    {
        public string Login { get; set; } = "";
        
    }
}
