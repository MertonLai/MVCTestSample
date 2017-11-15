using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.Models {

    [MetadataType(typeof(客戶聯絡人.Model))]
    public partial class 客戶聯絡人 : IValidatableObject {
        private class Model {

            [Required(ErrorMessage = "請輸入{0}資料")]
            public int Id { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            public int 客戶Id { get; set; }

            [UIHint("ContactJobTitle")]
            public string 職稱 { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            public string 姓名 { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            [DataType(DataType.EmailAddress, ErrorMessage = "{0}資料有錯誤，請輸入正確電子郵件格式")]
            [Remote("EmailIsUniquied", "CustContact", AdditionalFields = "客戶Id, Id", ErrorMessage = "客戶聯絡人電子郵件資料重複！")]
            public string Email { get; set; }

            [DataType(DataType.PhoneNumber)]
            public string 手機 { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            [DataType(DataType.PhoneNumber)]
            public string 電話 { get; set; }

            [Display(Name = "是否刪除")]
            [Required(ErrorMessage = "請輸入{0}資料")]
            public bool IsDelete { get; set; }

        }


        public bool CheckMailChanged(int id, string eMail) {
            bool resultValue = false;

            using (CustomEntities db = new CustomEntities()) {
                resultValue = db.客戶聯絡人.Any(c => c.Id == id && c.Email != Email.Trim());
            }

            return resultValue;
        }

        public bool CheckEmailIsUnique(int CustID, string eMail) {
            bool resultValue = false;
            using (CustomEntities db = new CustomEntities()) {
                resultValue = db.客戶聯絡人.Any(c => c.客戶Id == CustID && c.Email == Email);
            }

            return resultValue;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (!string.IsNullOrEmpty(this.Email)) {
                using (CustomEntities db = new CustomEntities()) {
                    if (db.客戶聯絡人.Any(c => c.IsDelete == false && c.客戶Id == this.客戶Id && c.Email == this.Email && c.Id != this.Id)) {
                        yield return new ValidationResult("同一客戶的聯絡人Email有重複", new string[] { "客戶Id", "Email" });
                    }
                }
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ContactJobTitle"></param>
        /// <param name="od"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public IEnumerable<客戶聯絡人> GetCustContData(string keyword, string ContactJobTitle, string od, string st) {
            IEnumerable<客戶聯絡人> data = new List<客戶聯絡人>();
            Models.CustomEntities db = new CustomEntities();

            data = db.客戶聯絡人.Where(c => c.IsDelete == false);

            if (!string.IsNullOrWhiteSpace(keyword)) {

                data = db.客戶聯絡人.Where(c => c.IsDelete == false || c.姓名.Contains(keyword) || c.客戶資料.客戶名稱.Contains(keyword) || c.客戶資料.統一編號.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(ContactJobTitle) && !ContactJobTitle.Equals("未設定")) {
                data = data.Where(c => c.職稱 == ContactJobTitle);
            }

            #region 排序處理

            switch (od) {
                case "職稱":
                    switch (st) {
                        case "D":
                            data = data.OrderByDescending(c => c.職稱);
                            break;
                        default:
                            data = data.OrderBy(c => c.職稱);
                            break;
                    }
                    break;
                case "姓名":
                    switch (st) {
                        case "D":
                            data = data.OrderByDescending(c => c.姓名);
                            break;
                        default:
                            data = data.OrderBy(c => c.姓名);
                            break;
                    }
                    break;
                case "Email":
                    switch (st) {
                        case "D":
                            data = data.OrderByDescending(c => c.Email);
                            break;
                        default:
                            data = data.OrderBy(c => c.Email);
                            break;
                    }
                    break;
                case "手機":
                    switch (st) {
                        case "D":
                            data = data.OrderByDescending(c => c.手機);
                            break;
                        default:
                            data = data.OrderBy(c => c.手機);
                            break;
                    }
                    break;
                case "電話":
                    switch (st) {
                        case "D":
                            data = data.OrderByDescending(c => c.電話);
                            break;
                        default:
                            data = data.OrderBy(c => c.電話);
                            break;
                    }
                    break;
                case "客戶名稱":
                    switch (st) {
                        case "D":
                            data = data.OrderByDescending(c => c.客戶資料.客戶名稱);
                            break;
                        default:
                            data = data.OrderBy(c => c.客戶資料.客戶名稱);
                            break;
                    }
                    break;

            }

            #endregion 排序處理

           

            return data;
        }

        public Dictionary<int, string> JobTitleList = new Dictionary<int, string>() {
            { 0, "未設定"},
            { 1, "一般職員"},
            { 2, "工程師"},
            { 3, "課長"},
            { 4, "經理"},
            { 5, "協理"},
            { 6, "副理"},
            { 7, "副總"},
            { 8, "總經理"},
            { 9, "處長"},
            { 10, "董事長"},
            { 11, "總裁"},
            { 12, "組長"},
            { 13, "副組長"},
            { 14, "助理工程師"},
            { 15, "副工程師"},
        };
    }

}