using News_Portal.Models.EntityData;
using News_Portal.Models.RSSFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace News_Portal.BLL
{
    public class CommonCode
    {
        public List<RSSFeed> HomeData()
        {
            var rssFeedList = new List<RSSFeed>();
            using (SystemDB db = new SystemDB())
            {
                var rssFeedBadwani = db.RSSFeed.Where(x => x.RSSFeedID == 1008).OrderByDescending(x => x.PublishDate).Take(5).ToList();
                foreach (var item in rssFeedBadwani)
                {
                    if (rssFeedBadwani[0].SliderData == null)
                    {
                        item.SliderData = SliderData();
                    }
                    rssFeedList.Add(item);
                }
                var rssFeedBhopal = db.RSSFeed.Where(x => x.RSSFeedID == 1010).OrderByDescending(x => x.PublishDate).Take(5).ToList();
                foreach (var item in rssFeedBhopal)
                {
                    rssFeedList.Add(item);
                }
                var rssFeedIndore = db.RSSFeed.Where(x => x.RSSFeedID == 1011).OrderByDescending(x => x.PublishDate).Take(5).ToList();
                foreach (var item in rssFeedIndore)
                {
                    rssFeedList.Add(item);
                }
                var rssFeedIntetnational = db.RSSFeed.Where(x => x.RSSFeedID == 1001).OrderByDescending(x => x.PublishDate).Take(6).ToList();
                foreach (var item in rssFeedIntetnational)
                {
                    rssFeedList.Add(item);
                }
                var rssFeedListBreaking = db.RSSFeed.Where(x => x.RSSFeedID == 1018).OrderByDescending(x => x.PublishDate).Take(10).ToList();
                foreach (var item in rssFeedListBreaking)
                {
                    rssFeedList.Add(item);
                }
            }
            return rssFeedList;
        }
        public List<RSSFeed> SliderData()
        {
            try
            {
                var rssFeedList = new List<RSSFeed>();
                using (SystemDB db = new SystemDB())
                {
                    var rssFeedListSlider = db.RSSFeed.Where(x => x.RSSFeedID == 1001).OrderByDescending(x => x.PublishDate).Take(10).ToList();
                    foreach (var item in rssFeedListSlider)
                    {
                        item.SliderData = new List<RSSFeed>();
                        item.SliderData.Add(item);
                        rssFeedList.Add(item);
                    }
                   // rssFeedList[0].SliderData.AddRange(item);
                    return rssFeedList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}