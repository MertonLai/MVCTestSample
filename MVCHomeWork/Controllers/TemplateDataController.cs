using MVCHomeWork.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.Controllers
{
    public class TemplateDataController : Controller {

        IBLL _BLL = new SysUtility();

        public IEnumerable<SelectListItem> CustCardList(int? SelectValue) {
            int _TmpData = 0;
            if (SelectValue.HasValue) {
                _TmpData = SelectValue.Value;
            }
            return _BLL.GetCustTypesList(_TmpData);


        }

        public string GetCustCartTypeName(int? SelectValue) {
            int _TmpData = 0;
            if (SelectValue.HasValue) {
                _TmpData = SelectValue.Value;
            }

            return _BLL.GetCustCartTypeName(_TmpData);
        }

        public string GetJobTitleName(string SelectText) {
            string _TmpData = "";
            if (!string.IsNullOrWhiteSpace(SelectText)) {
                _TmpData = SelectText;
            }
            return _BLL.GetJobTitleName(_TmpData);
        }

        public IEnumerable<SelectListItem> GetJobTitleList(string SelectText) {
            string _TmpData = "";
            if (!string.IsNullOrWhiteSpace(SelectText)) {
                _TmpData = SelectText;
            }
            return _BLL.GetJobTitleList(_TmpData);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                _BLL = null;
            }
            base.Dispose(disposing);
        }
    }
}