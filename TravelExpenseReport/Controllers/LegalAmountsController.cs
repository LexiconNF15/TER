using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelExpenseReport.Models;

namespace TravelExpenseReport.Controllers
{
    public class LegalAmountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LegalAmounts
        public ActionResult Index()
        {
            return View(db.LegalAmounts.ToList());
        }

        // GET: LegalAmounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LegalAmount legalAmount = db.LegalAmounts.Find(id);
            if (legalAmount == null)
            {
                return HttpNotFound();
            }
            return View(legalAmount);
        }

        // GET: LegalAmounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LegalAmounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LegalAmountId,ValidDate,FullDayAmount,HalfDayAmount,NightAmount,MilageAmount")] LegalAmount legalAmount)
        {
            if (ModelState.IsValid)
            {
                db.LegalAmounts.Add(legalAmount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(legalAmount);
        }

        // GET: LegalAmounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LegalAmount legalAmount = db.LegalAmounts.Find(id);
            if (legalAmount == null)
            {
                return HttpNotFound();
            }
            return View(legalAmount);
        }

        // POST: LegalAmounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LegalAmountId,ValidDate,FullDayAmount,HalfDayAmount,NightAmount,MilageAmount")] LegalAmount legalAmount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(legalAmount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(legalAmount);
        }

        // GET: LegalAmounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LegalAmount legalAmount = db.LegalAmounts.Find(id);
            if (legalAmount == null)
            {
                return HttpNotFound();
            }
            return View(legalAmount);
        }

        // POST: LegalAmounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LegalAmount legalAmount = db.LegalAmounts.Find(id);
            db.LegalAmounts.Remove(legalAmount);
            db.SaveChanges();
            return RedirectToAction("Index");
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
