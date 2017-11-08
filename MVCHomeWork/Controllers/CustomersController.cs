using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCHomeWork.Models;

namespace MVCHomeWork.Controllers
{
    public class CustomersController : Controller
    {
        private CustomEntities db = new CustomEntities();
        int PageCount = 20;
        // GET: Customers
        public ActionResult Index(string keyword, int? CustCardType, string od, string st) {

            CustomerViewModel model = new CustomerViewModel() {
                keyword = keyword,
                CustCardType = CustCardType,
                od = string.IsNullOrWhiteSpace(od) ? "" : od,
                st = string.IsNullOrWhiteSpace(st) ? "A" : st
            };

            ViewBag.CustCard = new BLL.SysUtility().GetCustTypesList((CustCardType.HasValue ? CustCardType.Value : 0));


            return View(model);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.客戶資料.Add(客戶資料);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: Customers/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = db.客戶資料.Find(id);

            var CustConst = db.客戶聯絡人.Where(c => c.客戶Id == id).AsEnumerable();
            foreach (var item in CustConst) {
                item.IsDelete = true;
            }

            var CustBank = db.客戶銀行資訊.Where(b => b.客戶Id == id).AsEnumerable();
            foreach (var item in CustBank) {
                item.IsDelete = true;
            }

            客戶資料.IsDelete = true;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult GridData(int? PageIndex, CustomerViewModel SearchData) {

            IEnumerable<客戶資料> Gmodel = new List<客戶資料>();

            if (string.IsNullOrEmpty(SearchData.keyword) && !SearchData.CustCardType.HasValue) {
                Gmodel = db.客戶資料.Where(c => c.IsDelete == false);
            } else {
                Gmodel = db.客戶資料.Where(c => c.IsDelete == false);

                // 查詢條件以名稱及統一編號當條件
                if (!string.IsNullOrWhiteSpace(SearchData.keyword)) {
                    Gmodel = Gmodel.Where(c => c.客戶名稱.Contains(SearchData.keyword) || c.統一編號.Contains(SearchData.keyword));
                }

                if (SearchData.CustCardType.HasValue && SearchData.CustCardType.Value > 0) {
                    Gmodel = Gmodel.Where(c => c.客戶分類 == SearchData.CustCardType.Value);
                }
            }

            #region 排序處理

            switch (SearchData.od) {
                case "客戶名稱":
                    switch (SearchData.st) {
                        case "D":
                            Gmodel = Gmodel.OrderByDescending(c => c.客戶名稱);
                            break;
                        default:
                            Gmodel = Gmodel.OrderBy(c => c.客戶名稱);
                            break;
                    }
                    break;
                case "電子郵件":
                    switch (SearchData.st) {
                        case "D":
                            Gmodel = Gmodel.OrderByDescending(c => c.Email);
                            break;
                        default:
                            Gmodel = Gmodel.OrderBy(c => c.Email);
                            break;
                    }
                    break;
                case "統一編號":
                    switch (SearchData.st) {
                        case "D":
                            Gmodel = Gmodel.OrderByDescending(c => c.統一編號);
                            break;
                        default:
                            Gmodel = Gmodel.OrderBy(c => c.統一編號);
                            break;
                    }
                    break;
                case "電話":
                    switch (SearchData.st) {
                        case "D":
                            Gmodel = Gmodel.OrderByDescending(c => c.電話);
                            break;
                        default:
                            Gmodel = Gmodel.OrderBy(c => c.電話);
                            break;
                    }
                    break;
                case "傳真":
                    switch (SearchData.st) {
                        case "D":
                            Gmodel = Gmodel.OrderByDescending(c => c.傳真);
                            break;
                        default:
                            Gmodel = Gmodel.OrderBy(c => c.傳真);
                            break;
                    }
                    break;
                case "地址":
                    switch (SearchData.st) {
                        case "D":
                            Gmodel = Gmodel.OrderByDescending(c => c.地址);
                            break;
                        default:
                            Gmodel = Gmodel.OrderBy(c => c.地址);
                            break;
                    }
                    break;
                case "客戶分類":
                    switch (SearchData.st) {
                        case "D":
                            Gmodel = Gmodel.OrderByDescending(c => c.客戶分類);
                            break;
                        default:
                            Gmodel = Gmodel.OrderBy(c => c.客戶分類);
                            break;
                    }
                    break;
            }

            #endregion 排序處理


            return PartialView("GridDataPartialView", Gmodel.Skip(PageIndex.HasValue ? PageIndex.Value + PageCount : 0).Take(PageCount));
        }


        public ActionResult CustomDetailList() {
            IEnumerable<CustomDetailVM> model;

            model = from C in db.客戶資料
                    select new CustomDetailVM() {
                     客戶名稱 = C.客戶名稱, 聯絡人數量 = C.客戶聯絡人.Count(), 銀行帳戶數量 = C.客戶銀行資訊.Count()
                    };

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
