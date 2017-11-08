using MVCHomeWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.BLL {
    public class SysUtility : IBLL {

        public IEnumerable<SelectListItem> GetCustTypesList(int SelectValue) {
            List<SelectListItem> returnValue = new List<SelectListItem>();

            var CustCardType = new 客戶資料().CustTypeList;

            foreach (var item in CustCardType) {
                returnValue.Add(new SelectListItem() {
                    Value = item.Key.ToString(),
                    Text = item.Value
                });
            }


            var data = returnValue.SingleOrDefault(s => s.Value == SelectValue.ToString());
            if (data != null) {
                data.Selected = true;
            } else {
                returnValue.SingleOrDefault(s => s.Value == "0").Selected = true;
            }



            return returnValue.AsEnumerable();
        }

        public string GetCustCartTypeName(int CardTypeValue) {
            var CustCardType = new 客戶資料().CustTypeList;
            var CardData = CustCardType.SingleOrDefault(s => s.Key == CardTypeValue);
            if (!CardData.Equals(null)) {
                return CardData.Value;
            } else {
                return "未設定";
            }
        }

        public IEnumerable<SelectListItem> GetJobTitleList(string SelectValue) {
            List<SelectListItem> returnValue = new List<SelectListItem>();

            var JobTitleList = new 客戶聯絡人().JobTitleList;

            foreach (var item in JobTitleList) {
                returnValue.Add(new SelectListItem() {
                    Value = item.Value,
                    Text = item.Value
                });
            }


            var data = returnValue.SingleOrDefault(s => s.Value == SelectValue);
            if (data != null) {
                data.Selected = true;
            } else {
                returnValue.SingleOrDefault(s => s.Value == "未設定").Selected = true;
            }



            return returnValue.AsEnumerable();
        }

        public string GetJobTitleName(string CardTypeValue) {
            var CustCardType = new 客戶聯絡人().JobTitleList;
            var CardData = CustCardType.SingleOrDefault(s => s.Value == CardTypeValue);
            if (!CardData.Equals(null)) {
                return CardData.Value;
            } else {
                return "未設定";
            }
        }
    }
}