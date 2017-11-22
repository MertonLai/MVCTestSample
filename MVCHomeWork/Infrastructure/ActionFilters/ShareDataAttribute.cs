using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.Infrastructure.ActionFilters {
    public class ShareDataAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            filterContext.Controller.ViewBag.Title = string.Format("{0}/{1}", filterContext.RouteData.Values["Controller"].ToString(), filterContext.RouteData.Values["Action"].ToString());


            base.OnActionExecuting(filterContext);
        }
    }
}