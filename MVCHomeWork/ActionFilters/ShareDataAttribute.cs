using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.ActionFilters {
    public class ShareDataAttribute : ActionFilterAttribute {
        public override void OnActionExecuted(ActionExecutedContext filterContext) {

            filterContext.Controller.ViewBag.Title = string.Format("{0}/{1}", filterContext.RouteData.Values["Controller"].ToString(), filterContext.RouteData.Values["Action"].ToString());


            base.OnActionExecuted(filterContext);
        }
    }
}