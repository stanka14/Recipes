using Data.DbContext;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AddUserAsync(ApplicationUser newUser)
        {
            try
            {
                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding a user.", ex);
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();
                return users.Select(user => new ApplicationUser()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PasswordHash = user.PasswordHash,
                    UserName = user.UserName
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting all users.", ex);
            }
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting user by email.", ex);
            }
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            try
            {
                return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting user by ID.", ex);
            }
        }

        public async Task UpdatePasswordAsync(ApplicationUser user, string newPassword)
        {
            try
            {
                user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, newPassword);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating user password.", ex);
            }
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating user.", ex);
            }
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
