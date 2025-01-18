using Microsoft.AspNetCore.Identity;

namespace Persistance.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Mail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
