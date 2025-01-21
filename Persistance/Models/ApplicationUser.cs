using Microsoft.AspNetCore.Identity;

namespace Persistance.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string RoleName { get; set; }
        public string Address { get; set; }
    }
}
