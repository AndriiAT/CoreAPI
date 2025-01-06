using Microsoft.AspNetCore.Identity;

namespace Persistance.Models
{
    internal class ApplicationRole : IdentityRole
    {
        // Additional properties
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
