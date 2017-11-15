using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCHomeWork.Models;
using MVCHomeWork.Infrastructure;
using MVCHomeWork.Infrastructure.CustomResults;
using Newtonsoft.Json;

namespace MVCHomeWork.Controllers
{
	public class CustContactController : BaseController
	{
		
        

        // GET: CustContact
        public ActionResult Index(string keyword, string ContactJobTitle, string od, string st)
		{
			ContactViewModel model = new ContactViewModel() {
				keyword = (string.IsNullOrEmpty(keyword) ? "" : keyword),
				ContactJobTitle = (string.IsNullOrEmpty(ContactJobTitle) ? "" : ContactJobTitle),
				od = (string.IsNullOrEmpty(od) ? "" : od),
				st = (string.IsNullOrEmpty(st) ? "A" : st)
			};

            model.GridModel = new 客戶聯絡人().GetCustContData(keyword, ContactJobTitle, od, st).Take(PageCount);

            ViewBag.JobTitleList = new BLL.SysUtility().GetJobTitleList(ContactJobTitle);

            return View(model);
		}

        

		// GET: CustContact/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
			if (客戶聯絡人 == null)
			{
				return HttpNotFound();
			}
			return View(客戶聯絡人);
		}

		// GET: CustContact/Create
		public ActionResult Create()
		{
			ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
			return View();
		}

		// POST: CustContact/Create
		// 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
		// 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
		{
			if (ModelState.IsValid)
			{
				// 驗證客戶聯繫資料是否有重複
				if (客戶聯絡人.CheckEmailIsUnique(客戶聯絡人.客戶Id,客戶聯絡人.Email)) {
					ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");

					ModelState.AddModelError("Email", "客戶Email資料重複，無法新增，請您確認一下客戶聯繫Email資料");
					return View(客戶聯絡人);
				} else {
					db.客戶聯絡人.Add(客戶聯絡人);
					db.SaveChanges();                    
				}
			}
			return RedirectToAction("Index");
		}

		// GET: CustContact/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
			if (客戶聯絡人 == null)
			{
				return HttpNotFound();
			}
			ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
			return View(客戶聯絡人);
		}

		// POST: CustContact/Edit/5
		// 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
		// 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
		{
			if (ModelState.IsValid)
			{
				客戶聯絡人.Email = 客戶聯絡人.Email.Trim();

				if (客戶聯絡人.CheckMailChanged(客戶聯絡人.Id, 客戶聯絡人.Email) && 客戶聯絡人.CheckEmailIsUnique(客戶聯絡人.客戶Id, 客戶聯絡人.Email)) {
					ModelState.AddModelError("Email", "客戶Email資料重複，無法新增，請您確認一下客戶聯繫Email資料");

					ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
					return View(客戶聯絡人);
				} else {
					db.Entry(客戶聯絡人).State = EntityState.Modified;
					db.SaveChanges();
				}
				return RedirectToAction("Index");
			}
			ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
			return View(客戶聯絡人);
		}

		// GET: CustContact/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
			if (客戶聯絡人 == null)
			{
				return HttpNotFound();
			}
			return View(客戶聯絡人);
		}

		// POST: CustContact/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人.IsDelete = true;
            db.SaveChanges();
			return RedirectToAction("Index");
		}

        /// <summary>
        /// 畫面即時驗證同一客戶的聯絡人電子郵件是否重複
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="客戶Id"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        public JsonResult EmailIsUniquied(int? Id, int 客戶Id, string Email) {

            if (db.客戶聯絡人.Any(c => c.IsDelete == false && c.Id != Id && c.客戶Id == 客戶Id && c.Email == Email)) {
                return Json(false, JsonRequestBehavior.AllowGet);
            } else {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }

		[ChildActionOnly]
		public ActionResult ContactInfo(int Id) {
			IEnumerable<客戶聯絡人> model = new List<客戶聯絡人>();

			model = db.客戶聯絡人.Where(t => t.客戶資料.Id == Id);

			return PartialView(model);
		}

        public ActionResult ExportData(string keyword, string ContactJobTitle, string od, string st) {
            var data = from C in new 客戶聯絡人().GetCustContData(keyword, ContactJobTitle, od, st).AsEnumerable()
                       select new {
                           客戶編號 = C.Id,
                           客戶名稱 = C.客戶資料.客戶名稱,
                           姓名 =C.姓名,
                           職稱 = _BLL.GetJobTitleName(C.職稱),
                           Email = C.Email,
                           電話 = C.電話,
                           是否刪除 = _BLL.GetDeleteStstus(C.IsDelete)
                       };


            var dt = LinqExtensions.LinqQueryToDataTable(data);
            //ListToDatatable.ListToDataTable<客戶資料>(data);


            var expFileName = string.Concat(
                "客戶聯絡人", DateTime.Now.ToString("yyyyMMddHHmmss"), ".xlsx"
                );

            return new ExportExcelResult {
                SheetName = "客戶聯絡人資料",
                ExportData = dt,
                FileName = expFileName
            };

        }


        //[ChildActionOnly]
        //public ActionResult GridData(int? PageIndex, ContactViewModel SearchData) {

        //    IEnumerable<客戶聯絡人> model = new List<客戶聯絡人>();

        //    if (string.IsNullOrWhiteSpace(SearchData.keyword)) {
        //        model = db.客戶聯絡人.Where(c => c.IsDelete == false);
        //    } else {
        //        model = db.客戶聯絡人.Where(c => c.IsDelete == false || c.姓名.Contains(SearchData.keyword) || c.客戶資料.客戶名稱.Contains(SearchData.keyword) || c.客戶資料.統一編號.Contains(SearchData.keyword));
        //    }

        //    if (!string.IsNullOrWhiteSpace(SearchData.ContactJobTitle) && !SearchData.ContactJobTitle.Equals("未設定")) {
        //        model = model.Where(c => c.職稱 == SearchData.ContactJobTitle);
        //    }

        //    #region 排序處理

        //    switch (SearchData.od) {
        //        case "職稱":
        //            switch (SearchData.st) {
        //                case "D":
        //                    model = model.AsEnumerable().OrderByDescending(c => c.職稱);
        //                    break;
        //                default:
        //                    model = model.OrderBy(c => c.職稱);
        //                    break;
        //            }
        //            break;
        //        case "姓名":
        //            switch (SearchData.st) {
        //                case "D":
        //                    model = model.OrderByDescending(c => c.姓名);
        //                    break;
        //                default:
        //                    model = model.OrderBy(c => c.姓名);
        //                    break;
        //            }
        //            break;
        //        case "Email":
        //            switch (SearchData.st) {
        //                case "D":
        //                    model = model.OrderByDescending(c => c.Email);
        //                    break;
        //                default:
        //                    model = model.OrderBy(c => c.Email);
        //                    break;
        //            }
        //            break;
        //        case "手機":
        //            switch (SearchData.st) {
        //                case "D":
        //                    model = model.OrderByDescending(c => c.手機);
        //                    break;
        //                default:
        //                    model = model.OrderBy(c => c.手機);
        //                    break;
        //            }
        //            break;
        //        case "電話":
        //            switch (SearchData.st) {
        //                case "D":
        //                    model = model.OrderByDescending(c => c.電話);
        //                    break;
        //                default:
        //                    model = model.OrderBy(c => c.電話);
        //                    break;
        //            }
        //            break;
        //        case "客戶名稱":
        //            switch (SearchData.st) {
        //                case "D":
        //                    model = model.OrderByDescending(c => c.客戶資料.客戶名稱);
        //                    break;
        //                default:
        //                    model = model.OrderBy(c => c.客戶資料.客戶名稱);
        //                    break;
        //            }
        //            break;
                    
        //    }

        //    #endregion 排序處理

        //    return PartialView("ContactGridDataPartialView", model.Skip(PageIndex.HasValue ? PageIndex.Value + PageCount : 0).Take(PageCount));
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
