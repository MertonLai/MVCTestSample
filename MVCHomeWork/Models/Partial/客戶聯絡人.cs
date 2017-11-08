using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.Models {

    [MetadataType(typeof(客戶聯絡人.Model))]
    public partial class 客戶聯絡人 {
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