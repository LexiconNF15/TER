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
    public class TravelReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TravelReports
        public ActionResult Index()
        {
            var travelReports = db.TravelReports.Include(t => t.ApplicationUser);
            return View(travelReports.ToList());
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
            ViewBag.Summa = travelReport.Night * legalAmount.NightAmount;
            return View(travelReport);
        }


        // GET: TravelReports/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName");
            return View();
        }

        // POST: TravelReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TravelReportId,ApplicationUserId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastReduction,LunchReduction,DinnerReduction,Status,Comment")] TravelReport travelReport)
        {
            if (ModelState.IsValid)
            {
                db.TravelReports.Add(travelReport);
                db.SaveChanges();
                return RedirectToAction("Edit2", new { id = travelReport.TravelReportId });
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName", travelReport.ApplicationUserId);
            return View(travelReport);
        }

        // GET: TravelReports/Create
        public ActionResult Create2()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName");
            return View();
        }

        // POST: TravelReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "TravelReportId,ApplicationUserId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastReduction,LunchReduction,DinnerReduction,Status,Comment")] TravelReport travelReport)
        {
            if (ModelState.IsValid)
            {
                db.TravelReports.Add(travelReport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName", travelReport.ApplicationUserId);
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
            return View(travelReport);
        }

        // GET: TravelReports/Edit/5
        public ActionResult Edit2(int? id)
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
            //ViewBag.TravelReportName1 = "2016-" + travelReport.TravelReportId.ToString().PadLeft(3, '0');
            travelReport.TravelReportName = "2016-" + travelReport.TravelReportId.ToString().PadLeft(3, '0');
            TimeSpan differense = travelReport.ReturnDate - travelReport.DepartureDate;

            travelReport.Night = differense.Days;
            travelReport.HalfDay = 0;
            travelReport.FullDay = travelReport.Night + 1;

            if (travelReport.DepartureTime.Hour >= 12)
            {
                travelReport.HalfDay++;
                travelReport.FullDay--;
            }

            if (travelReport.ReturnTime.Hour <= 18)
            {
                travelReport.HalfDay++;
                travelReport.FullDay--;
            }

            if (travelReport.ReturnTime.Hour <= 5)
            {
                travelReport.Night--;
                if (travelReport.Night < 0)
                {
                    travelReport.Night = 0;
                }
            }
            ViewBag.TravelReportId = travelReport.TravelReportId;

            return View(travelReport);
        }

        // POST: TravelReports/Edit2/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "TravelReportId,ApplicationUserId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastReduction,LunchReduction,DinnerReduction,Status,Comment")] TravelReport travelReport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(travelReport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName", travelReport.ApplicationUserId);
            return View(travelReport);
        }

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
