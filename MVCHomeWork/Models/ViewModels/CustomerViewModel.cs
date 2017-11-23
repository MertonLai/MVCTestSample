using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCHomeWork.Models {
    public class CustomerViewModel : CustomQueryVM {
        public CustomerViewModel() {
            GridModel = new List<客戶資料>();
        }
        
        //public string keyword {
        //    get;
        //    set;
        //}

        
        //public int? CustCardType {
        //    get;
        //    set;
        //}

        //public string od {
        //    get;
        //    set;
        //}

        //public string st {
        //    get; set;
        //}

        public IEnumerable<客戶資料> GridModel { get; set; }

    }
}