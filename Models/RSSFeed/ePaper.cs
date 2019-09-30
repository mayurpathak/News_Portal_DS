using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace News_Portal.Models.RSSFeed
{
    public class ePaper
    {
        [Key]
        public int ePaperID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public Nullable<DateTime> UploadedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}