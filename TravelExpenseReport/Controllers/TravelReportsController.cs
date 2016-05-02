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
    [Authorize]
    public class TravelReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TravelReports
        public ActionResult Index()
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();

            if (User.IsInRole("Assistant"))
            {
                var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Where(t => t.ApplicationUserId == ActiveUser.Id);
                return View(travelReports.ToList());
            }
            else
            {
                var travelReports = db.TravelReports.Include(t => t.ApplicationUser);
                return View(travelReports.ToList());
            }
        }

        // GET: TravelReports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelReport travelReport = db.TravelReports.Find(id);
            if (travelReport == null)
            {
                return HttpNotFound();
            }
            return View(travelReport);
        }

        // GET: TravelReports/Calculate/5
        //
        // The view Calculate will present the calculated sum of allowance for expenses
        //
        public ActionResult Calculate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelReport travelReport = db.TravelReports.Find(id);
            if (travelReport == null)
            {
                return HttpNotFound();
            }
            var legalAmount = db.LegalAmounts.FirstOrDefault();
            ViewBag.LegalAmount = legalAmount;

            int allowanceSum = 0;
            allowanceSum = allowanceSum + (int)travelReport.Night * (int)legalAmount.NightAmount;
            allowanceSum = allowanceSum + (int)travelReport.FullDay * (int)legalAmount.FullDayAmount;
            allowanceSum = allowanceSum + (int)travelReport.HalfDay * (int)legalAmount.HalfDayAmount;
            allowanceSum = allowanceSum - (int)travelReport.BreakfastReduction * (int)legalAmount.BreakfastReductionAmount;
            allowanceSum = allowanceSum - (int)travelReport.LunchReduction * (int)legalAmount.LunchReductionAmount;
            allowanceSum = allowanceSum - (int)travelReport.DinnerReduction * (int)legalAmount.DinnerReductionAmount;

            ViewBag.Summa = allowanceSum;

            var expensesThisTravel = db.Expenses.Where(e => e.TravelReportId == travelReport.TravelReportId);
            int noOfExpenses = expensesThisTravel.Count();
            float sumOfExpenses = 0;
            foreach (var e1 in expensesThisTravel)
            {
                sumOfExpenses = sumOfExpenses + (float)e1.ExpenseAmount;
            }

            ViewBag.NoOfExpenses = noOfExpenses;
            ViewBag.SummaPlus = allowanceSum + sumOfExpenses;

            return View(travelReport);
        }


        // GET: TravelReports/Create
        public ActionResult Create()
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();

            //ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.StatusName = db.StatusTypes.FirstOrDefault().StatusName;
            ViewBag.StatusTypeId1 = db.StatusTypes.Where(stt => stt.StatusName == "Ny").FirstOrDefault().StatusTypeId;
            ViewBag.ApplicationUserId1 = ActiveUser.Id;

            return View();
        }

        // POST: TravelReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TravelReportId,ApplicationUserId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastReduction,LunchReduction,DinnerReduction,StatusTypeId,Comment")] TravelReport travelReport)
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();

            string travelYear = travelReport.DepartureDate.Year.ToString();
            var TravelReportsSameYear = db.TravelReports.Where(t => t.ApplicationUserId == ActiveUser.Id && t.TravelReportName.Substring(0, 4) == travelYear).OrderByDescending(a => a.TravelReportName);
            int TravelReportNumber;
            if (TravelReportsSameYear.Count() == 0)
            {
                TravelReportNumber = 1;
            }
            else
            {
                TravelReportNumber = Int32.Parse(TravelReportsSameYear.FirstOrDefault().TravelReportName.Substring(5, 3));
                TravelReportNumber = TravelReportNumber + 1;
            }
            //travelReport.TravelReportName = "Testarnamn";
            travelReport.TravelReportName = travelYear + "-" + TravelReportNumber.ToString().PadLeft(3, '0');


            if (ModelState.IsValid)
            {
                db.TravelReports.Add(travelReport);
                db.SaveChanges();
                return RedirectToAction("Edit2", new { id = travelReport.TravelReportId });
            }

            ViewBag.StatusName = db.StatusTypes.FirstOrDefault().StatusName;
            ViewBag.StatusTypeId1 = db.StatusTypes.Where(stt => stt.StatusName == "Ny").FirstOrDefault().StatusTypeId;
            ViewBag.ApplicationUserId1 = ActiveUser.Id;

            return View(travelReport);
        }

        // GET: TravelReports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelReport travelReport = db.TravelReports.Find(id);
            if (travelReport == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName", travelReport.ApplicationUserId);
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName", travelReport.ApplicationUserId);
            return View(travelReport);
        }

        // POST: TravelReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TravelReportId,ApplicationUserId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastReduction,LunchReduction,DinnerReduction,Status,Comment")] TravelReport travelReport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(travelReport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName", travelReport.ApplicationUserId);
            ViewBag.StatusTypeId = new SelectList(db.StatusTypes, "StatusTypeId", "StatusName", travelReport.StatusTypeId);
            return View(travelReport);
        }

        // GET: TravelReports/Edit2/5
        public ActionResult Edit2(int? id)
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
            TravelReport travelReport = db.TravelReports.Find(id);
            //string travelYear = travelReport.DepartureDate.Year.ToString();
            //var TravelReportsSameYear = db.TravelReports.Where(t => t.ApplicationUserId == ActiveUser.Id && t.TravelReportName.Substring(0, 4) == travelYear).OrderByDescending(a => a.TravelReportName);
            //int TravelReportNumber;
            //if (TravelReportsSameYear.Count() == 0)
            //{
            //    TravelReportNumber = 1;
            //}
            //else
            //{
            //    TravelReportNumber = Int32.Parse(TravelReportsSameYear.FirstOrDefault().TravelReportName.Substring(5, 3));
            //    TravelReportNumber = TravelReportNumber + 1;
            //}

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (travelReport == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName", travelReport.ApplicationUserId);
            ViewBag.ApplicationUserId1 = ActiveUser.Id;
            ViewBag.StatusTypeId = new SelectList(db.StatusTypes, "StatusTypeId", "StatusName", travelReport.StatusTypeId);
            //travelReport.TravelReportName = travelYear + "-" + TravelReportNumber.ToString().PadLeft(3, '0');

            TimeSpan differense = travelReport.ReturnDate - travelReport.DepartureDate;

            travelReport.Night = differense.Days;
            if (travelReport.Night == 0)
            {
                travelReport.HalfDay = 0;
                travelReport.FullDay = 0;
                ViewBag.Traktamente = false;
            }
            else
            {
                travelReport.HalfDay = 0;
                travelReport.FullDay = travelReport.Night + 1;
                ViewBag.Traktamente = true;


                if (travelReport.DepartureTime.Hours >= 12)
                {
                    travelReport.HalfDay++;
                    travelReport.FullDay--;
                }

                if (travelReport.ReturnTime.Hours <= 18)
                {
                    travelReport.HalfDay++;
                    travelReport.FullDay--;
                }

                if (travelReport.ReturnTime.Hours <= 5)
                {
                    travelReport.Night--;
                    if (travelReport.Night < 0)
                    {
                        travelReport.Night = 0;
                    }
                }
            }
            ViewBag.TravelReportId = travelReport.TravelReportId;
            ViewBag.TravelReportName1 = travelReport.TravelReportName;


            return View(travelReport);
        }

        // POST: TravelReports/Edit2/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "TravelReportId,ApplicationUserId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastReduction,LunchReduction,DinnerReduction,StatusTypeId,Comment")] TravelReport travelReport, string button)
        {
            if (ModelState.IsValid)
            {
                db.Entry(travelReport).State = EntityState.Modified;
                db.SaveChanges();

                if (button == "Beräkna utan utgifter")
                {
                    return RedirectToAction("Calculate", new { id = travelReport.TravelReportId });
                }
                if (button ==  "Lägg till utgifter")
                {
                    return RedirectToAction("Create", "Expenses", new { id = travelReport.TravelReportId });
                }

                //return RedirectToAction("Index");
                return RedirectToAction("Calculate", new { id = travelReport.TravelReportId });

            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName", travelReport.ApplicationUserId);
            ViewBag.StatusTypeId = new SelectList(db.StatusTypes, "StatusTypeId", "StatusName", travelReport.StatusTypeId);
            return View(travelReport);
        }

        //
        //                if (ModelState.IsValid)
        //        {
        //            db.TravelReports.Add(travelReport);
        //            db.SaveChanges();
        //            return RedirectToAction("Edit2", new { id = travelReport.TravelReportId
        //});
        //        }

        //

        // GET: TravelReports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelReport travelReport = db.TravelReports.Find(id);
            if (travelReport == null)
            {
                return HttpNotFound();
            }
            return View(travelReport);
        }

        // POST: TravelReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TravelReport travelReport = db.TravelReports.Find(id);
            db.TravelReports.Remove(travelReport);
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
