using Asp.net_recepy_blog.Pages.Dbcontrol;

namespace Asp.net_recepy_blog.Pages.Services
{
    public interface IUserServices
    {
        Task<List<User>> GetUsersAsync();
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(int UserId);
        Task<List<User>> GetUsersByEmailAsync(string Email);
        Task<User> GetUserByIdAsync(int userId);
    }
}
