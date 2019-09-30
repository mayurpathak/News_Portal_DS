using System.Web.Mvc;

namespace News_Portal.Controllers
{
    public class ErrorHandlingController : Controller
    {
        public ActionResult UnAuthorized()
        {
            return View();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            //_Logger.Error(filterContext.Exception);

            //Redirect or return a view, but not both.
            filterContext.Result = RedirectToAction("Index", "ErrorHandler");
            // OR 
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Error/Error.cshtml"
            };
        }
    }
}