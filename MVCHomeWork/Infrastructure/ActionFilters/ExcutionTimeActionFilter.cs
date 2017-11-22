using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.Infrastructure.ActionFilters {
    public class ExcutionTimeActionFilter : ActionFilterAttribute {
        DateTime StartTime;
        DateTime EndTime;

        public override void OnResultExecuting(ResultExecutingContext filterContext) {
            this.EndTime = DateTime.Now;

            filterContext.Controller.ViewBag.ExecTime = DateTime.Compare(EndTime, StartTime);

            base.OnResultExecuting(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            this.StartTime = DateTime.Now;

            base.OnActionExecuting(filterContext);
        }
    }
}