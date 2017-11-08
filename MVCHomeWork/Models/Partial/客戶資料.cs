using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCHomeWork.Models {

    [MetadataType(typeof(客戶資料.Model))]
    public partial class 客戶資料 {


        private class Model {

            [Display(Name ="客戶編號")]
            public int Id { get; set; }


            [Required(ErrorMessage = "請輸入{0}資料")]
            public string 客戶名稱 { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            [Index(IsUnique = true)]
            [RegularExpression("^[0-9]*$", ErrorMessage ="{0} 只能是數字！！")]
            [StringLength(8, ErrorMessage ="{1} 只能輸入 {1} 碼")]
            public string 統一編號 { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            [DataType(DataType.PhoneNumber)]
            public string 電話 { get; set; }

            [DataType(DataType.PhoneNumber)]
            public string 傳真 { get; set; }

            [Required(ErrorMessage ="請輸入{0}資料")]
            [StringLength(100, ErrorMessage ="{0}只能輸入{1}個字數！！")]
            public string 地址 { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            [Display(Name ="電子郵件")]
            [DataType(DataType.EmailAddress, ErrorMessage ="{0}格式錯誤，請輸入正確電子郵件帳號")]
            public string Email { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            [Display(Name = "客戶分類")]
            [UIHint("CustCardType")]
            public int 客戶分類 { get; set; }

            [Display(Name ="是否刪除")]
            [Required(ErrorMessage = "請輸入{0}資料")]
            public bool IsDelete { get; set; }


        }


        /// <summary>
        /// 設定客戶類別設定檔
        /// </summary>
        /// <param name="SelectValue">選定代號</param>
        /// <returns></returns>
        

        public Dictionary<int, string> CustTypeList = new Dictionary<int, string>() {
            { 0, "未設定"},
            { 1, "白金級VIP"},
            { 2, "金級VIP"},
            { 3, "銀級VIP"},
            { 4, "銅級VIP"},
            { 5, "一般貴賓"},
            { 99, "黑名單"}
        };


    }
}