using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sesssion = (UserLogin)Session[CommonConstants.USER_SESSION];
            if(sesssion == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    System.Web.Routing.RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
            }
            base.OnActionExecuting(filterContext);
        }

        protected void SetAlert(string Message,string Type)
        {
            TempData["AlertMessage"] = Message;
            if (Type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (Type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (Type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}