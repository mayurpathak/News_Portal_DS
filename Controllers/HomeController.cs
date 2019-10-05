using News_Portal.BLL;
using News_Portal.Models;
using News_Portal.Models.EntityData;
using News_Portal.Models.RSSFeed;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace News_Portal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Home()
        {
            var rssFeedList = new List<RSSFeed>();
            using (SystemDB db = new SystemDB())
            {
                string ipAddress = GetIp();
                if (ipAddress != null)
                {
                    var userCount = db.UserCount.Where(x => x.IPAddess == ipAddress).FirstOrDefault();
                    UserCount userCount1 = new UserCount();
                    if (userCount == null)
                    {
                        userCount1.IPAddess = ipAddress;
                        userCount1.Count = 1;
                        db.Entry(userCount1).State = EntityState.Added;
                        db.SaveChanges();
                    };
                    int totalUserCount = db.UserCount.Count();
                    if (totalUserCount > 0)
                    {
                        Session["TotalUserCount"] = totalUserCount;
                    }
                }
                int[] rssids = new int[] { 1008, 1010, 1011 };
                CommonCode commonCode = new CommonCode();
                rssFeedList = commonCode.HomeData();
            }
            return View(rssFeedList);
        }
        //[HttpPost]
        //[Authorize]
        public ActionResult GetNews(int Id = 0)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    if (Convert.ToBoolean(Session["SuperAdmin"]))
                    {
                        var rssMasterList = db.RSSFeedMaster.ToList();
                        foreach (var items in rssMasterList)
                        {
                            try
                            {
                                string RSSURL = items.RSSURL;
                                XDocument xDoc = new XDocument();
                                xDoc = XDocument.Load(RSSURL);
                                //xDoc = XDocument.Load("test.xml");
                                var descriptions = xDoc.Descendants("description").ToList();
                                string descriptionCount = Convert.ToString(descriptions[1].FirstNode);

                                int Start, End;
                                if (descriptionCount.Contains("src=") && descriptionCount.Contains("/>"))
                                {
                                    Start = descriptionCount.IndexOf("src=", 0) + "src=".Length;
                                    End = descriptionCount.IndexOf("/>", Start);
                                    string imgpath = descriptionCount.Substring(Start, End - Start).Replace("\"", "").Replace("\"", "");
                                    //var url = "https://i10.dainikbhaskar.com/thumbnails/116x87/web2images/www.bhaskar.com/2019/08/29/0521_ms-dhoni_1.jpg";
                                    string[] imageNames = imgpath.Split('/').ToArray();
                                    string imageName = imageNames.Last();

                                    //using (WebClient client = new WebClient())
                                    //{
                                    //    string path = @"E:\RSSFeedWebApplication\RSSFeedMVC\Content\Images\" + imageName + "";
                                    //    client.DownloadFile(new Uri(imgpath), path);

                                    //    string imgBase64String = GetBase64StringForImage(path);
                                    //}
                                }


                                var RSSFeedData = (from x in xDoc.Descendants("item")
                                                   select new Feeds
                                                   {
                                                       Title = ((string)x.Element("title")),
                                                       Link = ((string)x.Element("link")),
                                                       Description = ((string)x.Element("description")),
                                                       PublishDate = ((string)x.Element("pubDate"))
                                                   }).ToList();

                                foreach (var item in RSSFeedData)
                                {
                                    string pubDate = item.PublishDate.Replace("\n\t   ", "").Replace(" GMT\t", "");
                                    DateTime publishDate = Convert.ToDateTime(pubDate);
                                    var dataCheck = db.RSSFeed.Where(x => x.PublishDate == publishDate && x.Title == item.Title).FirstOrDefault();
                                    if (dataCheck == null)
                                    {
                                        var rssFeedChild = new RSSFeed()
                                        {
                                            RSSFeedID = items.RSSFeedID,
                                            Title = item.Title,
                                            PublishDate = Convert.ToDateTime(pubDate),
                                            Description = item.Description,
                                            CreatedDate = DateTime.Now
                                        };
                                        db.RSSFeed.Add(rssFeedChild);
                                        db.SaveChanges();
                                    }
                                }
                                ViewBag.RSSFeed = RSSFeedData;
                                ViewBag.URL = Id;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("The remote server returned an error: (500) Internal Server Error."))
                                {
                                    continue;
                                }
                            }
                        }

                        CommonCode commonCode = new CommonCode();
                        var rssFeedList = commonCode.HomeData();
                        return View("~/Views/Home/Home.cshtml", rssFeedList);
                    }
                    else
                    {
                        return View("~/Views/Error/Unauthorized.cshtml");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string fileExtention = System.IO.Path.GetExtension(imgPath);
            fileExtention = fileExtention.Replace(".", "").TrimEnd();
            string base64String = "data:image/" + fileExtention + ";base64," + Convert.ToBase64String(imageBytes);
            return base64String;
        }
        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RSS()
        {
            string RssFeedUrl = "https://www.bhaskar.com/rss-feed/11215/";
            List<Feeds> feeds = new List<Feeds>();
            try
            {
                XDocument xDoc = new XDocument();
                xDoc = XDocument.Load(RssFeedUrl);
                var items = (from x in xDoc.Descendants("item")
                             select new
                             {
                                 title = x.Element("title").Value,
                                 link = x.Element("link").Value,
                                 pubDate = x.Element("pubDate").Value,
                                 description = x.Element("description").Value
                             });
                if (items != null)
                {
                    foreach (var i in items)
                    {
                        Feeds f = new Feeds
                        {
                            Title = i.title,
                            Link = i.link,
                            PublishDate = i.pubDate,
                            Description = i.description
                        };

                        feeds.Add(f);
                    }
                }
                return View(feeds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}