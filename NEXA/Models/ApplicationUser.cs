using Microsoft.AspNetCore.Identity;

namespace NEXA.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}
