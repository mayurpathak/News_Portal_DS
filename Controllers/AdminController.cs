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
                string username = fc.Get("Username");
                string password = fc.Get("Password");
                using (SystemDB db = new SystemDB())
                {
                    var login = db.Login.Where(x => x.Username == username && x.Password == password && x.IsDeleted != true).FirstOrDefault();
                    if (login != null)
                    {
                        if (login.Role == "SuperAdmin")
                        {
                            Session["SuperAdmin"] = true;
                        }
                        else
                        {
                            Session["Admin"] = true;
                        }
                        return View("~/Views/Home/Home.cshtml");
                    }
                    else
                    {
                        return View("~/Views/Home/Home.cshtml");
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
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/AddNews"), filename);
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
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/ePaper"), filename);
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