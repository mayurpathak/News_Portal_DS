using System.ComponentModel.DataAnnotations;

namespace News_Portal.Models.RSSFeed
{
    public class UserCount
    {
        [Key]
        public int IPID { get; set; }
        public string IPAddess { get; set; }
        public int Count { get; set; }
    }
}