using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Admin.Models
{
    public class AdminLogin
    {
        [Required]
        [Display(Name ="Username")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }
    }
}

