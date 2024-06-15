using Microsoft.EntityFrameworkCore;
using Asp.net_recepy_blog.Pages.Dbcontrol;
using Asp.net_recepy_blog.Pages.Services;

namespace Asp.net_recepy_blog.Pages.Services
{
    public class UserServices(MyDbContext context) : IUserServices
    {
        private readonly MyDbContext _context = context;

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Ошибка при сохранении пользователя: {ex.Message}");
                throw;
            }
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(int UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetUsersByEmailAsync(string email)
        {
            return await _context.Users
                .Where(user => user.Email == email)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.UserId == userId);
        }
    }
}
