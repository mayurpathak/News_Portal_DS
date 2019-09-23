﻿using News_Portal.Models;
using News_Portal.Models.EntityData;
using News_Portal.Models.RSSFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace News_Portal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Home(int RSSID = 0)
        {
            using (SystemDB db = new SystemDB())
            {
                var rssMasterList = db.RSSFeedMaster.ToList();
                foreach (var items in rssMasterList)
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
                    ViewBag.URL = RSSID;
                }
            }
            return View();
        }
        protected string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string fileExtention = System.IO.Path.GetExtension(imgPath);
            fileExtention = fileExtention.Replace(".", "").TrimEnd();
            string base64String = "data:image/" + fileExtention + ";base64," + Convert.ToBase64String(imageBytes);
            return base64String;
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