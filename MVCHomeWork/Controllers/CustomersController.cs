using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCHomeWork.Models;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using Newtonsoft.Json;
using MVCHomeWork.Infrastructure.CustomResults;
using MVCHomeWork.Infrastructure;

namespace MVCHomeWork.Controllers
{
    public class CustomersController : BaseController
    {


        // GET: Customers
        //public ActionResult Index(string keyword, int? CustCardType, string od, string st) {
        public ActionResult Index(string keyword, int? CustCardType, string od, string st) {

            CustomerViewModel model = new CustomerViewModel() {
                keyword = keyword,
                CustCardType = CustCardType,
                od = string.IsNullOrWhiteSpace(od) ? "" : od,
                st = string.IsNullOrWhiteSpace(st) ? "A" : st
            };

            ViewBag.CustCard = new BLL.SysUtility().GetCustTypesList((CustCardType.HasValue ? CustCardType.Value : 0));

            model.GridModel = new 客戶資料().GetCustomerData(model.keyword, model.CustCardType, model.od, model.st).Take(PageCount);

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
            客戶資料 model = new 客戶資料();
            return View(model);
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

        public ActionResult ExportData(string keyword, int? CustCardType, string od, string st) {
            var data = new 客戶資料().GetCustomerData(keyword, CustCardType, od, st).ToList();

            var dt = ListToDatatable.ListToDataTable<客戶資料>(data);


            var expFileName = string.Concat(
                "CustomerData", DateTime.Now.ToString("yyyyMMddHHmmss"), ".xlsx"
                );

            return new ExportExcelResult {
                SheetName = "客戶基本資料",
                ExportData = dt,
                FileName = expFileName
            };

        }

        //[ChildActionOnly]
        //public ActionResult GridData(int? PageIndex, CustomerViewModel SearchData) {

        //    IEnumerable<客戶資料> Gmodel = new List<客戶資料>();

        //    Gmodel = new 客戶資料().GetCustomerData(SearchData);


        //    return PartialView("GridDataPartialView", Gmodel.Skip(PageIndex.HasValue ? PageIndex.Value + PageCount : 0));
        //}



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
