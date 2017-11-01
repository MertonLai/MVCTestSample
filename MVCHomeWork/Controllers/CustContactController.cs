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
	public class CustContactController : Controller
	{
		private CustomEntities db = new CustomEntities();

		// GET: CustContact
		public ActionResult Index(string keyword)
		{
			IEnumerable<客戶聯絡人> model = new List<客戶聯絡人>();
			if (string.IsNullOrEmpty(keyword)) {
				model = db.客戶聯絡人.Include(c => c.客戶資料).Take(20);
			} else {
				model = db.客戶聯絡人.Include(c => c.客戶資料).Where(t => t.姓名.Contains(keyword) || t.客戶資料.客戶名稱.Contains(keyword) || t.客戶資料.統一編號.Contains(keyword)).Take(20);
			}
			

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
			db.客戶聯絡人.Remove(客戶聯絡人);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		public JsonResult EmailIsUniquied(int? Id, int 客戶Id, string Email) {
            if(db.客戶聯絡人.Any(c => c.Id == Id && c.Email == Email)) {
                return Json(true, JsonRequestBehavior.AllowGet);
            } else {
                if (db.客戶聯絡人.Any(c => c.客戶Id == 客戶Id && c.Email == Email)) {
                    return Json(false, JsonRequestBehavior.AllowGet);
                } else {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
			
		}

        [ChildActionOnly]
        public ActionResult ContactInfo(int Id) {
            IEnumerable<客戶聯絡人> model = new List<客戶聯絡人>();

            model = db.客戶聯絡人.Where(t => t.客戶資料.Id == Id);

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
