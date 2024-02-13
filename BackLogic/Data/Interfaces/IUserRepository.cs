using Data.Models;

namespace Data.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task AddUserAsync(ApplicationUser user);
        Task UpdateUserAsync(ApplicationUser user);
        Task UpdatePasswordAsync(ApplicationUser user, string newPassword);
    }
}
