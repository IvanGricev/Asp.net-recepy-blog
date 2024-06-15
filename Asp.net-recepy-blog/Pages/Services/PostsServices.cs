using Microsoft.EntityFrameworkCore;
using Asp.net_recepy_blog.Pages.Dbcontrol;

namespace Asp.net_recepy_blog.Pages.Services
{
    public class PostsServices(MyDbContext context) : IPostsServices
    {
        private readonly MyDbContext _context = context;

        public async Task<List<Posts>> GetPostAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Posts> AddPostAsync(Posts post)
        {
            _context.Posts.Add(post);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Ошибка при сохранении поста: {ex.Message}");
                throw;
            }
            return post;
        }

        public async Task<Posts> UpdatePostAsync(Posts post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Posts>> GetPostByUserIdAsync(int userid)
        {
            return await _context.Posts
                .Where(post => post.UserId == userid)
                .ToListAsync();
        }

        public async Task<Posts> GetPostByIdAsync(int postId)
        {
            return await _context.Posts.FirstOrDefaultAsync(post => post.PostId == postId);
        }
    }
}
