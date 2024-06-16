using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Asp.net_recepy_blog.Pages.Services;
using Asp.net_recepy_blog.Pages.Dbcontrol;
using System.Text.RegularExpressions;

namespace Asp.net_recepy_blog.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }
        private readonly IUserServices _userService;
        private readonly MyDbContext _context;
        private readonly EmailServices _emailService;

        [BindProperty]
        public string Action { get; set; }

        //Регистрация

        [BindProperty]
        public User User { get; set; }

        //Вход
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public LoginModel(MyDbContext context, EmailServices emailService, UserServices userService)
        {
            _context = context;
            _emailService = emailService;
            _userService = userService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Action == "register")
            {
                var existingUser = await _userService.GetUsersByEmailAsync(User.Email);
                if (existingUser.Any())
                {
                    ModelState.AddModelError("User.Email", "Пользователь с такой почтой уже существует.");
                    return Page();
                }

                if (!Regex.IsMatch(User.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).*$"))
                {
                    ModelState.AddModelError("User.Password", "Пароль должен содержать заглавные и строчные буквы, цифры и специальные символы");
                    return Page();
                }

                await _userService.AddUserAsync(User);
                await _emailService.SendEmailAsync(User.Email, "Поздравляем!", $"Теперь вы загегистрированный в нашей соицальной сети и сможете делится своими или читать чужие рецепты вкуснейшей еды!");
                return RedirectToPage("/Account");
            }
            else
            {
                var users = await _userService.GetUsersByEmailAsync(Email);
                if (users.Count > 0)
                {
                    var user = users.First();
                    if (user.Password == Password)
                    {
                        HttpContext.Session.SetString("UserId", user.UserId.ToString());
                        return RedirectToPage("/Account");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Не корректный пароль.");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Пользователь не найден.");
                    return Page();
                }
            }
        }
    }
}
