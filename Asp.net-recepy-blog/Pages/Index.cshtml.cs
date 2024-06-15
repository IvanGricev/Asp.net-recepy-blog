using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Asp.net_recepy_blog.Pages.Services;
using Asp.net_recepy_blog.Pages.Dbcontrol;

namespace Asp.net_recepy_blog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserServices _userService;
        private readonly MyDbContext _context;
        private readonly EmailServices _emailService;

        public IndexModel(ILogger<IndexModel> logger, MyDbContext context, EmailServices emailService, UserServices userService)
        {
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
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("Toggeled AddProject");
            Console.WriteLine($"Project: {newPost.Name}, {newPost.PostId}");
            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
