using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCHomeWork.BLL {
    public interface IBLL {
        IEnumerable<SelectListItem> GetCustTypesList(int SelectValue);

        string GetCustCartTypeName(int CardTypeValue);

        IEnumerable<SelectListItem> GetJobTitleList(string SelectValue);

        string GetJobTitleName(string CardTypeValue);

    }
}
