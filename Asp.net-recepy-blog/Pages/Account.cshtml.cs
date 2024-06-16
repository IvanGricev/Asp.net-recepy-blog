using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Asp.net_recepy_blog.Pages.Services;
using Asp.net_recepy_blog.Pages.Dbcontrol;

namespace Asp.net_recepy_blog.Pages
{
    public class AccountModel : PageModel
    {
        private readonly IUserServices _userService;
        private readonly MyDbContext _dbContext;
        private readonly EmailServices _emailService;

        public AccountModel(IUserServices userService, MyDbContext dbContext, EmailServices emailService)
        {
            _userService = userService;
            _dbContext = dbContext;
            _emailService = emailService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var user = await _userService.GetUserByIdAsync(userId);
                ViewData["User"] = user;
            }
            else
            {
                ViewData["User"] = null;
            }

            return Page();
        }

        [BindProperty]
        public User UserC { get; set; }
        public async Task<IActionResult> OnPostUpdatePasswordAsync(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (UserC.Name != null)
            {
                user.Name = UserC.Name;
            }
            if (UserC.Password != null)
            {
                user.Password = UserC.Password;
            }
            if (UserC.Email != null)
            {
                await _emailService.SendEmailAsync(user.Email, "Данные аккаунта изменились!", "Вы сменили свою электронную почту, теперь уведомления не будут приходить на эту почту.");
                user.Email = UserC.Email;
                await _emailService.SendEmailAsync(user.Email, "Данные аккаунта изменились!", "Это ваша новая электронная почта привязанная сайту по обмену рецептами.");
            }
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Account");
        }

        public async Task<IActionResult> OnPostDeleteAccountAsync(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(userId);
            HttpContext.Session.Clear();
            return RedirectToPage("/Account");
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Account");
        }

        //изменение запсей

        [BindProperty]
        public string PostName { get; set; }

        [BindProperty]
        public string Ingredients { get; set; }

        [BindProperty]
        public string Cookingmethod { get; set; }

        public async Task<IActionResult> OnPostUpdatePostAsync(int postId)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var user = await _userService.GetUserByIdAsync(userId);

                var post = await _dbContext.Posts.FindAsync(postId);
                if (post != null)
                {
                    post.Name = PostName;
                    post.Ingredients = Ingredients;
                    post.CookingMethod = Cookingmethod;
                    _dbContext.Posts.Update(post);
                }
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Account");
        }

        public async Task<IActionResult> OnPostDeletePostAsync(int postId)
        {
            var post = await _dbContext.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound();
            }

            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Account");
        }


    }
}
