using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelExpenseReport.Models;
using Microsoft.AspNet.Identity;

namespace TravelExpenseReport.Controllers
{   [Authorize]
    public class ExpensesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Expenses
        public ActionResult Index(int? id)
        {
            
            var activeUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();

            if (User.IsInRole("Assistant"))
            {
                var expenses = db.Expenses.Include(e => e.ExpenseType).Include(e => e.TravelReport).Where(e => e.TravelReport.ApplicationUserId == activeUser.Id);
                //var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Where(t => t.ApplicationUserId == activeUser.Id);
                return View(expenses.ToList());
            }
            else
            {
                var expenses = db.Expenses.Include(e => e.ExpenseType).Include(e => e.TravelReport);
                //var travelReports = db.TravelReports.Include(t => t.ApplicationUser);
                return View(expenses.ToList());
            };

            
            //var expenses = db.Expenses.Include(e => e.ExpenseType).Include(e => e.TravelReport);
            //ViewBag.ActualTravelReportId = expenses.FirstOrDefault().TravelReportId;
            //return View(expenses.ToList());


        }

        // GET: Expenses/Details/5
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

        // GET: Expenses/Create
        public ActionResult Create(int? id)
        {

            if (id != null)
            {
                //
            }
                ViewBag.ActualTravelReportId = id;
                Expense ex = new Expense();
                ex.TravelReportId = id;
                //TravelReport travelReport = db.TravelReports.Find(tId);
            //}
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName");
            //ViewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId", "TravelReportName");
            //ViewBag.ActualTravelReportId = id;
            return View(ex);
          
        }


        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpenseId,ExpenseTypeId,ExpenseInformation,ExpenseDate,ExpenseAmount,ExpenseMilage,TravelReportId")] Expense ex)
        {
            if (ModelState.IsValid)
            {
                db.Expenses.Add(ex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", ex.ExpenseTypeId);
            //ViewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId", expense.TravelReportId);
            return View(ex);
        }

        // GET: Expenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense ex = db.Expenses.Find(id);
            if (ex == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", ex.ExpenseTypeId);
            //ViewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId", expense.TravelReportId);
            //ViewBag.CurrentUserId = new SelectList(db.Users, "ApplicationUserId", "FullName");
            //ViewBag.TestId = new SelectList(db.TravelReports, " TravelReportId", "TravelReportName");
            return View(ex);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExpenseId,ExpenseTypeId,ExpenseInformation,ExpenseDate,ExpenseAmount,ExpenseMilage,TravelReportId,TravelReportName,FullName")] Expense ex)
        {

            if (ModelState.IsValid)
            {
                db.Entry(ex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", ex.ExpenseTypeId);
            //ViewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId", expense.TravelReportId);
            return View(ex);
        }

        // GET: Expenses/Delete/5
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

        // POST: Expenses/Delete/5
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

