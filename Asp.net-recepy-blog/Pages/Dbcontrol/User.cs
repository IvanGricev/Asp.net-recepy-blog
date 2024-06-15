using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Asp.net_recepy_blog.Pages.Dbcontrol
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [BindProperty]
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Username can only contain letters.")]
        public string Name { get; set; }

        [BindProperty]
        [Required]
        [StringLength(100, MinimumLength = 4)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{4,}$", ErrorMessage = "Password must meet complexity requirements.")]
        public string Password { get; set; }

        [BindProperty]
        public string Email { get; set; }

    }
}
