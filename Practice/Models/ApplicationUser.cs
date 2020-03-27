using Microsoft.AspNetCore.Identity;

namespace Practice.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
