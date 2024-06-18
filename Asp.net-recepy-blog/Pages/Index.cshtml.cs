using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Asp.net_recepy_blog.Pages.Services;
using Asp.net_recepy_blog.Pages.Dbcontrol;
using Microsoft.Extensions.Hosting.Internal;

namespace Asp.net_recepy_blog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserServices _userService;
        private readonly MyDbContext _context;
        private readonly EmailServices _emailService;
        private IWebHostEnvironment _hostingEnvironment;

        public IndexModel(ILogger<IndexModel> logger, MyDbContext context, EmailServices emailService, UserServices userService, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _emailService = emailService;
            _userService = userService;
            _logger = logger;
            newPost = new Posts();
        }

        [BindProperty]
        public Posts newPost { get; set; }

        public void OnGet()
        {

        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(IFormFile photo)
        {
            Console.WriteLine("Toggeled AddPost");
            Console.WriteLine($"Post: {newPost.Name}, {newPost.PostId}");
            if (photo != null && photo.Length > 0)
            {
                var webRootPath = _hostingEnvironment.WebRootPath;
                var filePath = Path.Combine(webRootPath, "imgs/", Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName));
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }
                    newPost.PhotoPath = Path.GetFileName(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка сохранения картинки: {ex.Message}");
                }

            }
            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

    }
}
