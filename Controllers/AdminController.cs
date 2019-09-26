using News_Portal.Models.EntityData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                string username = fc.Get("userName");
                string password = fc.Get("password");
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
        public ActionResult ADDNews()
        {
            return View();
        }
        public ActionResult UploadPaper()
        {
            return View();
        }
    }
}