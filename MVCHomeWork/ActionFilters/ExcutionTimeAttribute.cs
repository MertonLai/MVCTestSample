using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.ActionFilters {
    public class ExcutionTimeAttribute : ActionFilterAttribute {
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        /// <remarks>
        /// 這是用在要做LOG或是做後續通知處理使用
        /// </remarks>
        //public override void OnResultExecuted(ResultExecutedContext filterContext) {

        //    stopWatch.Stop();

        //    TimeSpan ts = stopWatch.Elapsed;

        //    filterContext.Controller.ViewBag.ExecTime = string.Format("回應時間：{0}", ts.ToString());

        //    base.OnResultExecuted(filterContext);
        //}

        public override void OnResultExecuting(ResultExecutingContext filterContext) {

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            filterContext.Controller.ViewBag.ExecTime = string.Format("回應時間：{0}", ts.ToString());


            base.OnResultExecuting(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            stopWatch.Start();

            base.OnActionExecuting(filterContext);
        }
    }
}