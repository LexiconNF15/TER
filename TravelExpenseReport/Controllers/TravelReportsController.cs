using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelExpenseReport.Models;
using TravelExpenseReport.ViewModels;

namespace TravelExpenseReport.Controllers
{

    [Authorize]
    public class TravelReportsController : Controller
    {
        public decimal SumOfAllowance(TravelReport travelReport)
        {
            var legalAmount = db.LegalAmounts.FirstOrDefault();
            int allowanceSum = 0;
            allowanceSum = allowanceSum + (int)travelReport.Night * (int)legalAmount.NightAmount;
            allowanceSum = allowanceSum + (int)travelReport.FullDay * (int)legalAmount.FullDayAmount;
            allowanceSum = allowanceSum + (int)travelReport.HalfDay * (int)legalAmount.HalfDayAmount;
            allowanceSum = allowanceSum - (int)travelReport.BreakfastReduction * (int)legalAmount.BreakfastReductionAmount;
            allowanceSum = allowanceSum - (int)travelReport.LunchReduction * (int)legalAmount.LunchReductionAmount;
            allowanceSum = allowanceSum - (int)travelReport.DinnerReduction * (int)legalAmount.DinnerReductionAmount;
            return (decimal)allowanceSum;
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TravelReports
        public ActionResult IndexOld()
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();

            if (User.IsInRole("Assistant"))
            {
                var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Where(t => t.ApplicationUserId == ActiveUser.Id).OrderBy(t => t.TravelReportName);
                return View(travelReports.ToList());
            }
            else
            {
                var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                var selection = new TravelReportViewModel();
                ViewBag.SelectUserId = new SelectList(db.Users, "Id", "FullName", ActiveUser.Id);
                return View(travelReports.ToList());

            }
        }

        public ActionResult Index(TravelReportViewModel1 selection)
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
            var _selection = selection;
            if (User.IsInRole("Assistant"))
            {
                var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Where(t => t.ApplicationUserId == ActiveUser.Id).OrderBy(t => t.TravelReportName);
                _selection.SelectedTRUser = travelReports;
                return View(selection);
            }
            else
            {
                if (selection.UserList == null)
                {
                    //var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                    var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Where(t=>t.ApplicationUser.CustomerId == ActiveUser.CustomerId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                    _selection.SelectedTRUser = travelReports;
                    var _selectiont1 = new TravelReportViewModel();

                    _selectiont1.TravelUsers = new SelectList(db.Users.Where(t =>t.CustomerId == ActiveUser.CustomerId), "Id", "FullName", ActiveUser.Id);

                    _selection.UserList = _selectiont1;

                }
                else
                {
                    var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Where(t => t.ApplicationUserId == selection.UserList.SelectedTravelUser).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                    _selection.SelectedTRUser = travelReports;
                    _selection.UserList.TravelUsers = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId), "Id", "FullName", ActiveUser.Id);
                }
                return View(_selection);

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

        // GET: TravelReports/Edit1/5
        public ActionResult Edit1(int? id)
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
            ViewBag.StatusTypeId = new SelectList(db.StatusTypes, "StatusTypeId", "StatusName", travelReport.StatusTypeId);
            return View(travelReport);
        }

        // POST: TravelReports1/Edit1/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit1([Bind(Include = "TravelReportId,ApplicationUserId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastReduction,LunchReduction,DinnerReduction,StatusTypeId,Comment")] TravelReport travelReport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(travelReport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit2", new { id = travelReport.TravelReportId });

            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName", travelReport.ApplicationUserId);
            ViewBag.StatusTypeId = new SelectList(db.StatusTypes, "StatusTypeId", "StatusName", travelReport.StatusTypeId);
            return View(travelReport);
        }

        // GET: TravelReports/Calc/5
        //
        // The view Calculate will present the calculated sum of allowance for expenses
        //
        public ActionResult Calc(int? id)
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
            //var legalAmount = db.LegalAmounts.FirstOrDefault();
            var legalAmount = db.LegalAmounts.Where(l => l.ValidDate <= travelReport.DepartureDate).OrderByDescending(l => l.ValidDate).FirstOrDefault();
            ViewBag.LegalAmount = legalAmount;

            var sumOfAll = SumOfAllowance(travelReport);
            ViewBag.Summa = sumOfAll;

            var expensesThisTravel = db.Expenses.Where(e => e.TravelReportId == travelReport.TravelReportId);
            int noOfExpenses = expensesThisTravel.Count();
            decimal sumOfExpenses = 0;
            foreach (var e1 in expensesThisTravel)
            {
                sumOfExpenses = sumOfExpenses + (decimal)e1.ExpenseAmount;
            }

            ViewBag.NoOfExpenses = noOfExpenses;
            ViewBag.SumOfExpenses = sumOfExpenses;

            ViewBag.SummaPlus = sumOfAll + sumOfExpenses;

            ViewBag.TravelReportId = travelReport.TravelReportId;

            ViewBag.Traktamente = (travelReport.Night != 0);


            return View(travelReport);
        }


        // POST: TravelReports/Calc/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Calc([Bind(Include = "TravelReportId,ApplicationUserId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastReduction,LunchReduction,DinnerReduction,StatusTypeId,Comment")] TravelReport travelReport, string button)
        {
            if (ModelState.IsValid)
            {

                if (button == "Ändra traktamente")
                {
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Edit2", new { id = travelReport.TravelReportId });
                }
                if (button == "Lägg till utgifter")
                {
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Create", "Expenses", new { tId = travelReport.TravelReportId });
                }
                if (button == "Skicka in")
                {
                    travelReport.StatusTypeId = db.StatusTypes.Where(stt => stt.StatusName == "Inskickad").FirstOrDefault().StatusTypeId;
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                if (button == "Till listan")
                {
                    return RedirectToAction("Index");
                }
                if (button == "Ändra datum/tid")
                {
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Edit1", new { id = travelReport.TravelReportId });
                }

                return RedirectToAction("Calc", new { id = travelReport.TravelReportId });

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

                if (button == "Summera")
                {
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Calc", new { id = travelReport.TravelReportId });
                }
                if (button == "Lägg till utgifter")
                {
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Create", "Expenses", new { tId = travelReport.TravelReportId });
                }
                if (button == "Skicka in")
                {
                    travelReport.StatusTypeId = db.StatusTypes.Where(stt => stt.StatusName == "Inskickad").FirstOrDefault().StatusTypeId;
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                if (button == "Ändra datum/tid")
                {
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Edit1");
                }

                return RedirectToAction("Index");

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
            //var legalAmount = db.LegalAmounts.FirstOrDefault();
            var legalAmount = db.LegalAmounts.Where(l => l.ValidDate <= travelReport.DepartureDate).OrderByDescending(l => l.ValidDate).FirstOrDefault();
            ViewBag.LegalAmount = legalAmount;

            var sumOfAll = SumOfAllowance(travelReport);
            ViewBag.Summa = sumOfAll;

            var expensesThisTravel = db.Expenses.Where(e => e.TravelReportId == travelReport.TravelReportId);
            int noOfExpenses = expensesThisTravel.Count();
            decimal sumOfExpenses = 0;
            foreach (var e1 in expensesThisTravel)
            {
                sumOfExpenses = sumOfExpenses + (decimal)e1.ExpenseAmount;
            }

            ViewBag.NoOfExpenses = noOfExpenses;
            ViewBag.SummaPlus = sumOfAll + sumOfExpenses;

            ViewBag.TravelReportId = travelReport.TravelReportId;

            return View(travelReport);
        }

        // POST: TravelReports/Calculate/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Calculate([Bind(Include = "TravelReportId,ApplicationUserId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastReduction,LunchReduction,DinnerReduction,StatusTypeId,Comment")] TravelReport travelReport, string button)
        {
            if (ModelState.IsValid)
            {

                if (button == "Summera")
                {
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Calculate", new { id = travelReport.TravelReportId });
                }
                if (button == "Lägg till utgifter")
                {
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Create", "Expenses", new { tId = travelReport.TravelReportId });
                }
                if (button == "Skicka in")
                {
                    travelReport.StatusTypeId = db.StatusTypes.Where(stt => stt.StatusName == "Inskickad").FirstOrDefault().StatusTypeId;
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

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
