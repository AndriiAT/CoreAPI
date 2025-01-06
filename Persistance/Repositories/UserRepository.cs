using Persistance.Context;
using Persistance.DTOs;
using Persistance.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;

        public UserRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUserDTO> CreateAsync(ApplicationUserDTO user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            var userModel = new ApplicationUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfCreation = user.DateOfCreation,
                Address = user.Address
            };

            _context.Users.Add(userModel);
            await _context.SaveChangesAsync();

            user.Id = userModel.Id;
            return user;
        }

        public async Task<ApplicationUserDTO> ReadAsync(string userId)
        {
            var userModel = await _context.Users.FindAsync(int.Parse(userId));
            if (userModel == null)
            {
                return null;
            }

            return new ApplicationUserDTO
            {
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                DateOfCreation = userModel.DateOfCreation,
                Address = userModel.Address
            };
        }

        public async Task<ApplicationUserDTO> UpdateAsync(ApplicationUserDTO user)
        {
            var userModel = await _context.Users.FindAsync(user.Id);
            if (userModel == null)
            {
                return null;
            }

            userModel.FirstName = user.FirstName;
            userModel.LastName = user.LastName;
            userModel.Email = user.Email;
            userModel.DateOfCreation = user.DateOfCreation;
            userModel.Address = user.Address;

            _context.Users.Update(userModel);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task DeleteAsync(string userId)
        {
            var userModel = await _context.Users.FindAsync(int.Parse(userId));
            if (userModel != null)
            {
                _context.Users.Remove(userModel);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ApplicationUserDTO>> ReadAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(user => new ApplicationUserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfCreation = user.DateOfCreation,
                Address = user.Address
            });
        }
    }
}
