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
using System.Text.RegularExpressions;

namespace TravelExpenseReport.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Check ExpenseAmountInfo
        public decimal AmountCheck(string expenseAmountInfo)
        {

            String AmountRegex = @"[0-9]+,?[0-9]?[0-9]?";
            if (Regex.IsMatch(expenseAmountInfo, AmountRegex))
            {
                // "Amount is valid convert from string to decimal
                //expense.ExpenseAmount = Decimal.Parse(expense.ExpenseAmountInfo);
                return Decimal.Parse(expenseAmountInfo);
            }
            else
            {
                // "Amount is invalid, expense.ExpenseAmount = 0;
                return 0;
            }
        }

        // GET: Expenses
        public ActionResult Index(int tId)
        {
            var activeUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
            ViewBag.ActualTravelReportId = tId;
            var expenses = db.Expenses.Include(e => e.ExpenseType).Include(e => e.TravelReport).Where(e => e.TravelReportId == tId);
            TravelReport activeTravelReport = db.TravelReports.Find(tId);
            ViewBag.ActiveTravelReport = activeTravelReport;
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ActualTravelReportId = expense.TravelReportId;
            return View(expense);
        }

        // GET: Expenses/Create
        public ActionResult Create(int tId)
        {
            string faultMessage = null;

            TravelReport activeTravelReport = db.TravelReports.Find(tId); ;
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName");
            ViewBag.ActualTravelReportId = tId;
            ViewBag.ActiveTravelReport = activeTravelReport;
            ViewBag.ErrorMsg = faultMessage;
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpenseId,ExpenseTypeId,ExpenseDescription,ExpenseDate,ExpenseAmountInfo,ExpenseAmount,ExpenseMilage,TravelReportId")] Expense expense)
        {
            string faultMessage = null;

            TravelReport activeTravelReport = db.TravelReports.Find(expense.TravelReportId); ;
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName");
            ViewBag.ActualTravelReportId = expense.TravelReportId;
            ViewBag.ActiveTravelReport = activeTravelReport;

            if (expense.ExpenseTypeId == 4) // 4 = Driving own car. 
            {
                if (expense.ExpenseMilage > 0)
                {
                    // Calulate: ExpenseAmount = ExpenseMilage * Milage from LegalAmount for valid year(devided by 100).
                    var activeLegalMilage = db.LegalAmounts.Where(l => l.ValidDate <= expense.ExpenseDate).OrderBy(l => l.ValidDate).FirstOrDefault();
                    DateTime actualValidDate = DateTime.Parse("2013-01-01");
                    float actualLegalMilageAmount = 0;
                    expense.ExpenseAmount = 0;
                    actualLegalMilageAmount = activeLegalMilage.MilageAmount;
                    expense.ExpenseAmount = (decimal)((actualLegalMilageAmount) * (expense.ExpenseMilage) / 100);

                }
                else if ((expense.ExpenseMilage == 0) || (expense.ExpenseMilage == null))
                {
                    // There is no amount in ExpenseMilage. Error message in Tempdata.
                    expense.ExpenseAmount = 0;
                    expense.ExpenseAmountInfo = null;
                    expense.ExpenseMilage = 0;
                    faultMessage = "Resa med bil, ange antal kilometer";
                    ViewBag.ErrorMsg = faultMessage;
                    ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", expense.ExpenseTypeId);
                    return View(expense);
                }
            }
            if (expense.ExpenseTypeId != 4)
            {
                if ((expense.ExpenseAmountInfo == null))
                {
                    expense.ExpenseMilage = 0;
                    expense.ExpenseAmount = 0;
                    faultMessage = "Ange kostnad för utgiften";
                    ViewBag.ErrorMsg = faultMessage;
                    ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", expense.ExpenseTypeId);
                    return View(expense);
                }
                else if (expense.ExpenseAmountInfo != null)
                {
                    expense.ExpenseMilage = 0;
                    expense.ExpenseAmount = AmountCheck(expense.ExpenseAmountInfo);
                }
            }

            if (ModelState.IsValid)
            {
                db.Expenses.Add(expense);
                db.SaveChanges();
                return RedirectToAction("Index", new { tId = expense.TravelReportId });
            }
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", expense.ExpenseTypeId);
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
            TravelReport activeTravelReport = db.TravelReports.Find(expense.TravelReportId);
            if (expense == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", expense.ExpenseTypeId);
            ViewBag.ActualTravelReportId = expense.TravelReportId;
            ViewBag.ActualExpenseTypeId = expense.ExpenseTypeId;
            ViewBag.ActiveTravelReport = activeTravelReport;
            var actualExpenseTypeName = db.ExpenseTypes.Where(e => e.ExpenseTypeId == expense.ExpenseTypeId).FirstOrDefault();
            ViewBag.ExpenseTypeName = actualExpenseTypeName.ExpenseTypeName;
            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExpenseId,ExpenseTypeId,ExpenseDescription,ExpenseDate,ExpenseAmountInfo,ExpenseAmount,ExpenseMilage,TravelReportId,,TravelReportName,FullName")] Expense expense)
        {
            string faultMessage = null;

            ViewBag.ActualTravelReportId = expense.TravelReportId;
            var actualExpenseTypeName = db.ExpenseTypes.Where(e => e.ExpenseTypeId == expense.ExpenseTypeId).FirstOrDefault();
            ViewBag.ExpenseTypeName = actualExpenseTypeName.ExpenseTypeName;
            TravelReport activeTravelReport = db.TravelReports.Find(expense.TravelReportId);
            ViewBag.ActiveTravelReport = activeTravelReport;

            if (expense.ExpenseTypeId == 4) // 4 = Driving own car. Payment per milage driven
            {
                if (expense.ExpenseMilage > 0)
                {
                    // Calulate: ExpenseAmount = ExpenseMilage * Milage from LegalAmount for valid year.
                    var activeLegalMilage = db.LegalAmounts.Where(l => l.ValidDate <= expense.ExpenseDate).OrderBy(l => l.ValidDate).FirstOrDefault();
                    DateTime actualValidDate = DateTime.Parse("2013-01-01");
                    float actualLegalMilageAmount = 0;
                    expense.ExpenseAmount = 0;
                    actualLegalMilageAmount = activeLegalMilage.MilageAmount;
                    expense.ExpenseAmount = (decimal)((actualLegalMilageAmount) * (expense.ExpenseMilage) / 100);

                }
                else if ((expense.ExpenseMilage == 0) || (expense.ExpenseMilage == null))
                {
                    // There is no amount in ExpenseMilage. Error message.
                    expense.ExpenseAmount = 0;
                    expense.ExpenseAmountInfo = null;
                    expense.ExpenseMilage = 0;
                    faultMessage = "Resa med bil, ange antal kilometer";
                    ViewBag.ErrorMsg = faultMessage;
                    return View(expense);
                }
            }

            if (expense.ExpenseTypeId != 4)
            {
                if ((expense.ExpenseAmountInfo == null))
                {
                    expense.ExpenseMilage = 0;
                    expense.ExpenseAmount = 0;
                    faultMessage = "Ange kostnad för utgiften";
                    ViewBag.ErrorMsg = faultMessage;
                    ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", expense.ExpenseTypeId);
                    return View(expense);
                }
                else if (expense.ExpenseAmountInfo != null)
                {
                    expense.ExpenseMilage = 0;
                    expense.ExpenseAmount = AmountCheck(expense.ExpenseAmountInfo);
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { tId = expense.TravelReportId });
            }
            //ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "ExpenseTypeId", "ExpenseTypeName", expense.ExpenseId);
            ViewBag.ActualTravelReportId = expense.TravelReportId;
            ViewBag.ActualExpenseTypeId = expense.ExpenseTypeId;
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
            return RedirectToAction("Index", new { tId = actualTravelReportId });
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