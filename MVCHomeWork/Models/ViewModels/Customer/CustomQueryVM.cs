﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCHomeWork.Models {
    public class CustomQueryVM {

        [Display(Name = "查詢條件", Prompt = "客戶名稱或統一編號")]
        public string keyword {
            get;
            set;
        }

        [Display(Name = "客戶分類")]
        [UIHint("CustCardType")]
        public int? CustCardType {
            get;
            set;
        }
        public string od {
            get;
            set;
        }

        public string st {
            get; set;
        }

        
    }
}