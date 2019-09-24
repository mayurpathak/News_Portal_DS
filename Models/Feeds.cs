using News_Portal.Models.RSSFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace News_Portal.Models
{
    public class Feeds
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string PublishDate { get; set; }
        public string Description { get; set; }
        public List<RSSFeedMaster> RSSFeedMaster { get; set; }
        public string Base64Image { get; set; }
    }
}