using MVCHomeWork.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.ActionFilters {
    public class CustCardTypeAttribute : ActionFilterAttribute {
        protected IBLL _BLL = new SysUtility();
        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            int? CustCardType = filterContext.RouteData.Values["CustCardType"] as int?;

            filterContext.Controller.ViewBag.CustCard = _BLL.GetCustTypesList((CustCardType.HasValue ? CustCardType.Value : 0));

            base.OnActionExecuting(filterContext);
        }
    }
}