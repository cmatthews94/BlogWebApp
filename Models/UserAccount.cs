using System.ComponentModel.DataAnnotations;

namespace BlogWebApp.Models
{
    public class UserAccount
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
