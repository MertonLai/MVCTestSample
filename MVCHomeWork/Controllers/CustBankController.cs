using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCHomeWork.Models;
using Newtonsoft.Json;
using MVCHomeWork.Infrastructure.ActionResults;
using MVCHomeWork.Infrastructure.Helpers;
using MVCHomeWork.ActionFilters;
using System.Data.Entity.Validation;

namespace MVCHomeWork.Controllers
{
    public class CustBankController : BaseController {

        

        // GET: CustBank
        public ActionResult Index(string keyword)
        {
            keyword = "";

            IEnumerable<客戶銀行資訊> model = new List<客戶銀行資訊>();

            if (TempData["BankSearchKey"] != null) {
                keyword = TempData["BankSearchKey"] as string;
            }

            TryUpdateModel(keyword);

            TempData["BankSearchKey"] = ViewBag.SearchKey = keyword;

            model = new 客戶銀行資訊().GetCustBankData(keyword).Take(PageCount);

            return View(model);
        }

        [AjaxOnly]
        public ActionResult GetBankData(string keyword) {

            TempData["BankSearchKey"] = ViewBag.SearchKey = keyword;

            var GridModel = (from B in new 客戶銀行資訊().GetCustBankData(keyword).Take(PageCount).AsEnumerable()
                             select new {
                                 Id = B.Id,
                                 銀行名稱 = B.銀行名稱,
                                 銀行代碼 = B.銀行代碼,
                                 分行代碼 = B.分行代碼,
                                 帳戶名稱 = B.帳戶名稱,
                                 帳戶號碼 = B.帳戶號碼,
                                 客戶名稱 = B.客戶資料.客戶名稱
                             }).ToList();


            var data = JsonConvert.SerializeObject(GridModel, Formatting.None);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ExportData(string keyword) {
            var data = from B in new 客戶銀行資訊().GetCustBankData(keyword).Take(PageCount).AsEnumerable()
                       select new {
                           ID = B.Id,
                           銀行名稱 = B.銀行名稱,
                           銀行代碼 = B.銀行代碼,
                           分行代碼 = B.分行代碼,
                           帳戶名稱 = B.帳戶名稱,
                           帳戶號碼 = B.帳戶號碼,
                           客戶名稱 = B.客戶資料.客戶名稱,
                           是否刪除 = _BLL.GetDeleteStstus(B.IsDelete)
                       };


            var dt = LinqExtensions.LinqQueryToDataTable(data);

            var expFileName = string.Concat(
                "客戶銀行資訊", DateTime.Now.ToString("yyyyMMddHHmmss"), ".xlsx"
                );

            return new ExportExcelResult {
                SheetName = "客戶銀行資訊",
                ExportData = dt,
                FileName = expFileName
            };

        }

        // GET: CustBank/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: CustBank/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
            return View();
        }

        // POST: CustBank/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                db.客戶銀行資訊.Add(客戶銀行資訊);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        public ActionResult CreateNew() {
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "ErrorDbEntityValidationException")]
        public ActionResult CreateNew([Bind(Include = "客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊) {
            try {
                TryUpdateModel(客戶銀行資訊);
                db.客戶銀行資訊.Add(客戶銀行資訊);
                db.SaveChanges();

            } catch (DbEntityValidationException dex) {
                throw dex;
            } 
            catch (Exception ex) {

                throw ex;
            }
            

            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return RedirectToAction("Index");

        }

        // GET: CustBank/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: CustBank/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                db.Entry(客戶銀行資訊).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: CustBank/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: CustBank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            客戶銀行資訊.IsDelete = true;
            //db.客戶銀行資訊.Remove(客戶銀行資訊);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult BankInfo(int Id) {
            IEnumerable<客戶銀行資訊> model = new List<客戶銀行資訊>();

            model = db.客戶銀行資訊.Where(b => b.客戶資料.Id == Id);

            return PartialView(model);
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
