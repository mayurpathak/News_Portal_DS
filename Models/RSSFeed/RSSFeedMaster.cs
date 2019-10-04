using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News_Portal.Models.RSSFeed
{
    public partial class RSSFeedMaster
    {
        [Key]
        public int RSSFeedID { get; set; }
        public string RSSName { get; set; }
        public string RSSURL { get; set; }
        public Nullable<System.DateTime> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedBy { get; set; }
        [NotMapped]
        public List<RSSFeedDDL> RSSFeedDDL { get; set; }
    }
}