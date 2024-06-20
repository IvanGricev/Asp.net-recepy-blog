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

        [BindProperty]
        public IFormFile photo { get; set; }

        public void OnGet()
        {

        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("Toggeled AddPost");
            Console.WriteLine($"Post: {newPost.Name}, {newPost.PostId}");
            if (photo != null && photo.Length > 0)
            {
                var fileName = Path.GetFileName(photo.FileName);
                string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "imgs");

                var filePath = Path.Combine(directoryPath, fileName);
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(fileStream);
                    }
                    newPost.PhotoPath = Path.GetFileName(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка сохранения картинки: {ex.Message}");
                    // Обработка исключения, например, можно установить путь к запасной картинке
                    newPost.PhotoPath = "default.png";
                }
            }
            else
            {
                newPost.PhotoPath = "default.png";
            }
            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }


    }
}
