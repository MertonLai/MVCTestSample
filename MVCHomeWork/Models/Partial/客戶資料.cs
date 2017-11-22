using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic;

namespace MVCHomeWork.Models {

    [MetadataType(typeof(客戶資料.Model))]
    public partial class 客戶資料 {

        private class Model {

            [Display(Name = "客戶編號")]
            public int Id { get; set; }


            [Required(ErrorMessage = "請輸入{0}資料")]
            public string 客戶名稱 { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            [Index(IsUnique = true)]
            [RegularExpression("^[0-9]*$", ErrorMessage = "{0} 只能是數字！！")]
            [StringLength(8, ErrorMessage = "{1} 只能輸入 {1} 碼")]
            public string 統一編號 { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            [DataType(DataType.PhoneNumber)]
            public string 電話 { get; set; }

            [DataType(DataType.PhoneNumber)]
            public string 傳真 { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            [StringLength(100, ErrorMessage = "{0}只能輸入{1}個字數！！")]
            public string 地址 { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            [Display(Name = "電子郵件")]
            [DataType(DataType.EmailAddress, ErrorMessage = "{0}格式錯誤，請輸入正確電子郵件帳號")]
            public string Email { get; set; }

            [Required(ErrorMessage = "請輸入{0}資料")]
            [Display(Name = "客戶分類")]
            [UIHint("CustCardType")]
            public int 客戶分類 { get; set; }

            [Display(Name = "是否刪除")]
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

        

        public IEnumerable<客戶資料> GetCustomerData(string keyword, int? CustCardType, string od, string st) {
            IEnumerable<客戶資料> data = new List<客戶資料>();

            Models.CustomEntities db = new CustomEntities();


            data = db.客戶資料.Where(c => c.IsDelete == false);
            if (!string.IsNullOrEmpty(keyword) || CustCardType.HasValue) {

                // 查詢條件以名稱及統一編號當條件
                if (!string.IsNullOrWhiteSpace(keyword)) {
                    data = data.Where(c => c.客戶名稱.Contains(keyword) || c.統一編號.Contains(keyword));
                }

                if (CustCardType.HasValue && CustCardType.Value > 0) {
                    data = data.Where(c => c.客戶分類 == CustCardType.Value);
                }
            }

            #region 排序處理
            if (!string.IsNullOrEmpty(od)) {
                data = data.OrderBy(od + (st != null && st.Equals("D") ? " DESC" : ""));
            }
            //switch (od) {
                
            //    case "客戶名稱":
            //        switch (st) {
            //            case "D":
            //                data = data.OrderByDescending(c => c.客戶名稱);
            //                break;
            //            default:
            //                data = data.OrderBy(c => c.客戶名稱);
            //                break;
            //        }
            //        break;
            //    case "電子郵件":
            //        switch (st) {
            //            case "D":
            //                data = data.OrderByDescending(c => c.Email);
            //                break;
            //            default:
            //                data = data.OrderBy(c => c.Email);
            //                break;
            //        }
            //        break;
            //    case "統一編號":
            //        switch (st) {
            //            case "D":
            //                data = data.OrderByDescending(c => c.統一編號);
            //                break;
            //            default:
            //                data = data.OrderBy(c => c.統一編號);
            //                break;
            //        }
            //        break;
            //    case "電話":
            //        switch (st) {
            //            case "D":
            //                data = data.OrderByDescending(c => c.電話);
            //                break;
            //            default:
            //                data = data.OrderBy(c => c.電話);
            //                break;
            //        }
            //        break;
            //    case "傳真":
            //        switch (st) {
            //            case "D":
            //                data = data.OrderByDescending(c => c.傳真);
            //                break;
            //            default:
            //                data = data.OrderBy(c => c.傳真);
            //                break;
            //        }
            //        break;
            //    case "地址":
            //        switch (st) {
            //            case "D":
            //                data = data.OrderByDescending(c => c.地址);
            //                break;
            //            default:
            //                data = data.OrderBy(c => c.地址);
            //                break;
            //        }
            //        break;
            //    case "客戶分類":
            //        switch (st) {
            //            case "D":
            //                data = data.OrderByDescending(c => c.客戶分類);
            //                break;
            //            default:
            //                data = data.OrderBy(c => c.客戶分類);
            //                break;
            //        }
            //        break;
            //}

            #endregion 排序處理

 
            return data;
        }


        public IEnumerable<CustomDetailVM> GetCustomDetailData() {
            IEnumerable<CustomDetailVM> data = new List<CustomDetailVM>();

            CustomEntities db = new CustomEntities();

            data = from C in db.客戶資料
                   select new CustomDetailVM() {
                       客戶名稱 = C.客戶名稱,
                       聯絡人數量 = C.客戶聯絡人.Count(),
                       銀行帳戶數量 = C.客戶銀行資訊.Count()
                   };


            return data;
        }

    }
}