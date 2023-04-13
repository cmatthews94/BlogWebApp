using System.ComponentModel.DataAnnotations;

namespace BlogWebApp.Models
{
    public class UserAccount
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
