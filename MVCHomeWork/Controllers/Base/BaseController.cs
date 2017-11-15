using MVCHomeWork.BLL;
using MVCHomeWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.Controllers
{
    public abstract class BaseController : Controller
    {
        protected CustomEntities db = new CustomEntities();
        protected int PageCount = 20;

        protected IBLL _BLL = new SysUtility();

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}