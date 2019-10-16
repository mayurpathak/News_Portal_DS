using News_Portal.Models.EntityData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using News_Portal.Models.RSSFeed;
using System.Data.Entity;

namespace News_Portal.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            try
            {
                var rssFeedList = new List<RSSFeed>();
                string username = fc.Get("Username");
                string password = fc.Get("Password");
                using (SystemDB db = new SystemDB())
                {
                    var login = db.Login.Where(x => x.Username == username && x.Password == password && x.IsDeleted != true).FirstOrDefault();
                    if (login != null)
                    {
                        var rssFeedBadwani = db.RSSFeed.Where(x => x.RSSFeedID == 1008).OrderByDescending(x => x.PublishDate).Take(5).ToList();
                        foreach (var item in rssFeedBadwani)
                        {
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
                        if (login.Role == "SuperAdmin")
                        {
                            Session["SuperAdmin"] = true;
                        }
                        else
                        {
                            Session["Admin"] = true;
                        }
                        return View("~/Views/Home/Home.cshtml",rssFeedList);
                    }
                    else
                    {
                        return View("~/Views/Home/Home.cshtml",rssFeedList);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            Session["SuperAdmin"] = null;
            return RedirectToAction("Home", "Home");
        }
        public ActionResult ADDNews()
        {
            try
            {
                if (Convert.ToBoolean(Session["SuperAdmin"]))
                {
                    RSSFeed rssMasterList = new RSSFeed();
                    using (SystemDB db = new SystemDB())
                    {
                        rssMasterList.RSSFeedDDL = (from ss in db.RSSFeedMaster
                                                    select new RSSFeedDDL
                                                    {
                                                        RSSFeedID = ss.RSSFeedID,
                                                        RSSName = ss.RSSName
                                                    }).ToList();
                    }
                    return View(rssMasterList);
                }
                else
                {
                    return View("~/Views/Error/Unauthorized.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ADDNews(RSSFeed model, HttpPostedFileBase file)
        {
            try
            {
                if (Convert.ToBoolean(Session["SuperAdmin"]))
                {
                    string filename = System.IO.Path.GetFileName(file.FileName);
                    //string path = System.IO.Path.Combine(
                    //                       Server.MapPath("~/Content/AddNews"), filename);
                    string filePaths = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\AddNews\\");
                    string path = filePaths +"\\" + filename + "";
                    // file is uploaded
                    file.SaveAs(path);
                    RSSMasterController RSSOBJ = new RSSMasterController();
                    string image = RSSOBJ.GetBase64StringForImage(path);
                    using (SystemDB db = new SystemDB())
                    {
                        model.CreatedBy = 1000;
                        model.CreatedDate = DateTime.Now;
                        model.Image = image;
                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();

                        model.RSSFeedDDL = (from ss in db.RSSFeedMaster
                                            select new RSSFeedDDL
                                            {
                                                RSSFeedID = ss.RSSFeedID,
                                                RSSName = ss.RSSName
                                            }).ToList();
                    }
                }
                else
                {
                    return View("~/Views/Error/Unauthorized.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }
        public ActionResult ePaper()
        {
            try
            {
                if (Convert.ToBoolean(Session["SuperAdmin"]))
                {
                    return View();
                }
                else
                {
                    return View("~/Views/Error/Unauthorized.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ePaper(ePaper model, HttpPostedFileBase file)
        {
            try
            {
                if (Convert.ToBoolean(Session["SuperAdmin"]))
                {
                    string filename = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/ePaper"), filename);
                    file.SaveAs(path);
                    using (SystemDB db = new SystemDB())
                    {
                        model.CreatedBy = 1000;
                        model.CreatedDate = DateTime.Now;
                        model.FileName = filename;
                        model.FilePath = path;
                        model.IsActive = true;
                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();

                    }
                    return View();
                }
                else
                {
                    return View("~/Views/Error/Unauthorized.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}