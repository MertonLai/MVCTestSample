using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCHomeWork.Models {
    public class ContactViewModel : ContactSearchVM {

        public ContactViewModel() {
            GridModel = new List<客戶聯絡人>();
        }

        public IEnumerable<客戶聯絡人> GridModel { get; set; }
    }
}