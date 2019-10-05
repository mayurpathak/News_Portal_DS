using News_Portal.Models.EntityData;
using News_Portal.Models.RSSFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace News_Portal.Controllers
{
    public class GetHomeController : Controller
    {
        public ActionResult NationalDataRetrive(int Id=0)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    var rssFeedList = new List<RSSFeed>();
                    rssFeedList = db.RSSFeed.Where(x => x.RSSFeedID == Id).OrderByDescending(x => x.PublishDate).Take(5).ToList();
                    return PartialView("NationalData", rssFeedList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SportsDataRetrive(int Id = 0)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    var rssFeedList = new List<RSSFeed>();
                    rssFeedList = db.RSSFeed.Where(x => x.RSSFeedID == Id).OrderByDescending(x => x.PublishDate).Take(3).ToList();
                    return PartialView("SportsData", rssFeedList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult BollywoodDataRetrive(int Id = 0)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    var rssFeedList = new List<RSSFeed>();
                    rssFeedList = db.RSSFeed.Where(x => x.RSSFeedID == Id).OrderByDescending(x => x.PublishDate).Take(3).ToList();
                    return PartialView("SportsData", rssFeedList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult BreakingNewsDataRetrive(int Id = 0)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    var rssFeedList = new List<RSSFeed>();
                    rssFeedList = db.RSSFeed.Where(x => x.RSSFeedID == Id).OrderByDescending(x => x.PublishDate).Take(10).ToList();
                    return PartialView("BreakingNewsData", rssFeedList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}