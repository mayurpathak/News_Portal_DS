using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public List<RSSFeedDDL> RSSFeedDDL { get; set; }
        [NotMapped]
        public List<RSSFeed> SliderData { get; set; }
    }
}