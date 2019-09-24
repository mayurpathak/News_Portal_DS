using System;
using System.ComponentModel.DataAnnotations;

namespace News_Portal.Models.RSSFeed
{
    public partial class RSSFeed
    {
        [Key]
        public int RSSID { get; set; }
        public int RSSFeedID { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public System.DateTime PublishDate { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedBy { get; set; }
    }
}