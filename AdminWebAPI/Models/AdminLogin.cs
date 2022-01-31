using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AdminWebAPI.Models
{
    public class AdminLogin
    {
        public string Username { get; set; }

        public string PasswordHash { get; set; }
    }
}

