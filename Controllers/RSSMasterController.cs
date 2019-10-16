using News_Portal.Models.EntityData;
using News_Portal.Models.RSSFeed;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace News_Portal.Controllers
{
    public class RSSMasterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult RSSMenuData(int RSSFeedID,string from="")
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    RSSFeed rSSFeed = new RSSFeed();
                    DateTime testingDate = db.RSSFeed.Where(x => x.RSSFeedID == RSSFeedID).ToList().OrderByDescending(x => x.PublishDate).Select(x => x.PublishDate).FirstOrDefault();
                    RSSFeedMaster rSSFeedMaster = db.RSSFeedMaster.Where(x => x.RSSFeedID == RSSFeedID).FirstOrDefault();
                    var rssFeedList = new List<RSSFeed>();
                    if (rSSFeedMaster.RSSName == "Horoscope")
                    {
                        rssFeedList = db.RSSFeed.Where(x => x.RSSFeedID == RSSFeedID && DbFunctions.TruncateTime(x.PublishDate) == testingDate.Date).ToList();
                    }
                    else
                    {
                        rssFeedList = db.RSSFeed.Where(x => x.RSSFeedID == RSSFeedID).OrderByDescending(x => x.PublishDate).Take(20).ToList();
                    }
                    if (Convert.ToBoolean(Session["SuperAdmin"]))
                    {
                        DownloadImages(RSSFeedID,from);
                    }
                    return View("~/Views/RSSMaster/News.cshtml", rssFeedList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult RSSChildData(int RSSFeedID, int RSSID)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    RSSFeed rSSFeed = new RSSFeed();
                    DateTime testingDate = db.RSSFeed.Where(x => x.RSSFeedID == RSSFeedID).ToList().OrderByDescending(x => x.PublishDate).Select(x => x.PublishDate).FirstOrDefault();
                    var rssFeedList = db.RSSFeed.Where(x => x.RSSFeedID == RSSFeedID && x.RSSID == RSSID && DbFunctions.TruncateTime(x.PublishDate) == testingDate.Date).FirstOrDefault();
                    rssFeedList.Description.Replace("\"", "");                  
                    return View("~/Views/RSSMaster/Description.cshtml", rssFeedList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string DownloadImages(int RSSFeedID = 0,string from="")
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    RSSFeed rSSFeed = new RSSFeed();
                    DateTime testingDate = db.RSSFeed.Where(x => x.RSSFeedID == RSSFeedID).ToList().OrderByDescending(x => x.PublishDate).Select(x => x.PublishDate).FirstOrDefault();
                    List<RSSFeed> rssFeedList = db.RSSFeed.Where(x => x.RSSFeedID == RSSFeedID && DbFunctions.TruncateTime(x.PublishDate) == testingDate.Date).ToList();

                    foreach (var item in rssFeedList)
                    {
                        try
                        {

                        string descriptionCount = item.Description;
                        int Start, End;
                        if (descriptionCount.Contains("src=") && descriptionCount.Contains("/>"))
                        {
                            if (from != "")
                            {
                                Start = descriptionCount.IndexOf("<img src=", 0) + "<img src=".Length;
                                End = descriptionCount.IndexOf(">", Start);
                            }
                            else
                            {
                                Start = descriptionCount.IndexOf("src=", 0) + "src=".Length;
                                End = descriptionCount.IndexOf("/>", Start);
                            }
                            string imgpath = descriptionCount.Substring(Start, End - Start).Replace("\"", "").Replace("\"", "");                            
                            string[] imageNames = imgpath.Split('/').ToArray();
                            string imageName = imageNames.Last();
                            string imgBase64String = string.Empty;
                            using (WebClient client = new WebClient())
                            {
                                //string path = @"E:\RSSFeedWebApplication\RSSFeedMVC\Content\Images\" + imageName + "";
                                //string filePaths = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory+ "\\Content\\Images\\");
                                //string path = filePaths +"\\" + imageName + "";
                                string path = System.IO.Path.Combine(Server.MapPath("~/Content/images"), imageName);
                                client.DownloadFile(new Uri(imgpath), path);
                                imgBase64String = GetBase64StringForImage(path);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                            }

                            var rssFeedChild = db.RSSFeed.Where(x => x.RSSID == item.RSSID).FirstOrDefault();
                            {
                                //RSSID = item.RSSID,
                                RSSFeedID = item.RSSFeedID;
                                rssFeedChild.Image = imgBase64String;
                                rssFeedChild.Title = item.Title;
                                rssFeedChild.PublishDate = item.PublishDate;
                                rssFeedChild.Description = item.Description;
                                rssFeedChild.CreatedDate = item.CreatedDate;
                            }
                            db.Entry(rssFeedChild).State = EntityState.Modified;
                            db.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("404"))
                            {
                                continue;
                            }
                            throw ex;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string fileExtention = System.IO.Path.GetExtension(imgPath);
            fileExtention = fileExtention.Replace(".", "").TrimEnd();
            string base64String = "data:image/" + fileExtention + ";base64," + Convert.ToBase64String(imageBytes);
            return base64String;
        }
    }
}