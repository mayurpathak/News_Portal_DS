using System.ComponentModel.DataAnnotations;

namespace News_Portal.Models.RSSFeed
{
    public class Login
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public string Role { get; set; }

    }
}