using Asp.net_recepy_blog.Pages.Dbcontrol;

namespace Asp.net_recepy_blog.Pages.Services
{
    public interface IPostsServices
    {
        Task<List<Posts>> GetPostAsync();
        Task<Posts> AddPostAsync(Posts post);
        Task<Posts> UpdatePostAsync(Posts post);
        Task DeletePostAsync(int PostId);
        Task<List<Posts>> GetPostByUserIdAsync(int userid);
        Task<Posts> GetPostByIdAsync(int postId);
    }
}
