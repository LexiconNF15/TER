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
    public class Expenses1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Expenses1
        public ActionResult Index()
        {
            var expenses = db.Expenses.Include(e => e.ExpenseType).Include(e => e.TravelReport);
            return View(expenses.ToList());
        }

        // GET: Expenses1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // GET: Expenses1/Create
        public ActionResult Create()
        {
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName");
            ViewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId");
            return View();
        }

        // POST: Expenses1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpenseId,ExpenseTypeId,ExpenseInformation,ExpenseDate,ExpenseAmount,ExpenseMilage,TravelReportId")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                db.Expenses.Add(expense);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", expense.ExpenseTypeId);
            ViewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId", expense.TravelReportId);
            return View(expense);
        }

        // GET: Expenses1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", expense.ExpenseTypeId);
            ViewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId", expense.TravelReportId);
            return View(expense);
        }

        // POST: Expenses1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExpenseId,ExpenseTypeId,ExpenseInformation,ExpenseDate,ExpenseAmount,ExpenseMilage,TravelReportId")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", expense.ExpenseTypeId);
            ViewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId", expense.TravelReportId);
            return View(expense);
        }

        // GET: Expenses1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // POST: Expenses1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expense expense = db.Expenses.Find(id);
            db.Expenses.Remove(expense);
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
