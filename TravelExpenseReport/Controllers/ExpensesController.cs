﻿using System;
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
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Expenses
        public ActionResult Index(int tId)
        {
            var activeUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
            var expenses = db.Expenses.Include(e => e.ExpenseType).Include(e => e.TravelReport).Where(e => e.TravelReportId == tId);
            ViewBag.ActiveUser = activeUser;
            ViewBag.ActualTravelReportId = tId;
            return View(expenses.ToList());
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
            ViewBag.ActualTravelReportId = expense.TravelReportId;
            return View(expense);
        }

        // GET: Expenses/Create
        public ActionResult Create(int tId)
        {
            var activeTravelReport = db.TravelReports.Where(tr => tr.TravelReportId == tId);

            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName");
            //ViewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId","TravelReportName");
            ViewBag.ActualTravelReportId = tId;
            return View();
        }
        
        // POST: Expenses/Create
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
                return RedirectToAction("Index", new { tId = expense.TravelReportId, id = expense.ExpenseId });
            }

            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", expense.ExpenseTypeId);
            //iewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId", ex.TravelReportId);
            return View(expense);
        }

        // GET: Expenses/Edit/5
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
            ViewBag.ActualTravelReportId = expense.TravelReportId;
            //ViewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId", ex.TravelReportId);
            //ViewBag.CurrentUserId = new SelectList(db.Users, "ApplicationUserId", "FullName");
            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExpenseId,ExpenseTypeId,ExpenseInformation,ExpenseDate,ExpenseAmount,ExpenseMilage,TravelReportId,TravelReportName,FullName")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { tId = expense.TravelReportId });
            }
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", expense.ExpenseId);
            //ViewBag.TravelReportId = new SelectList(db.TravelReports, "TravelReportId", "ApplicationUserId", expense.TravelReportId);
            return View(expense);
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
            ViewBag.ActualTravelReportId = expense.TravelReportId;
            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expense expense = db.Expenses.Find(id);
            int? actualTravelReportId = expense.TravelReportId;
            db.Expenses.Remove(expense);
            db.SaveChanges(); 
            return RedirectToAction("Index",new { tId = actualTravelReportId});
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