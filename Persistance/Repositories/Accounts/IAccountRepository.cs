using Persistance.Models;

namespace ProductsShop.Repositories.Accounts
{
    public interface IAccountRepository
    {
        Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);
        Task<ApplicationUser> FindByEmailAsync(string email);
    }
}
