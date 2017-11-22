﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.Infrastructure.ActionFilters {
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute {
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo) {
            return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}