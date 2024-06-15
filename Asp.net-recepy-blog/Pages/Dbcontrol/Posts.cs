using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Asp.net_recepy_blog.Pages.Dbcontrol
{
    public class Posts
    {
        [Key]
        public int PostId { get; set; }

        [BindProperty]
        [Required]
        public int UserId { get; set; }

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        public string PhotoPath { get; set; } = "default.png";

        [BindProperty]
        public string Ingredients { get; set; }

        [BindProperty]
        public string CookingMethod { get; set; }

    }
}
