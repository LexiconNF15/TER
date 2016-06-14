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
            allowanceSum = allowanceSum - (int)travelReport.BreakfastDeduction * (int)legalAmount.BreakfastAmount;
            allowanceSum = allowanceSum - (int)travelReport.LunchOrDinnerDeduction * (int)legalAmount.LunchOrDinnerAmount;
            allowanceSum = allowanceSum - (int)travelReport.LunchAndDinnerDeduction * (int)legalAmount.LunchAndDinnerAmount;
            allowanceSum = allowanceSum - (int)travelReport.AllMealsDeduction * (int)legalAmount.AllMealsAmount;
            return (decimal)allowanceSum;
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(TravelReportViewModel1 selection)
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
            ViewBag.ActiveUser = ActiveUser.Id;
            var LenaUser = ActiveUser.Id;
            //var LenaPatients1 = db.PatientUsers.Where(t => t.StaffUserId == LenaUser).Include(t => t.Patient).Where(t => t.PatientId == t.Patient.PatientId && t.Patient.CustomerId == ActiveUser.CustomerId);
            //var LenaPatients1 = db.PatientUsers.Where(t => t.StaffUserId == LenaUser).Include(t => t.Patient).Where(t => t.PatientId == t.Patient.PatientId && t.Patient.CustomerId == ActiveUser.CustomerId).Include(t=>t.TravelReport).Where(t=>t.Patient.PatientId == t.TravelReprort.PatientId);
            //var LenaPatients = db.Patients.Where(t => t.CustomerId == ActiveUser.CustomerId).Select(t => t.PatientUser);

            //var StaffPatientsUsers = db.PatientUsers.Where(u => u.StaffUserId == ActiveUser.Id ).ToList().FirstOrDefault();
            ////var TRreports = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", ActiveUser.Id);
            var TRreports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);


            //foreach (var tr in TRreports)
            //{
            //    if (tr.PatientId == LenaPatients1.PatientId)
            //    {
            //        // lägg till i selectlistan
            //    }
            //}


            var _selection = selection;

            if (User.IsInRole("Assistant"))
            {
                var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == ActiveUser.Id).OrderBy(t => t.TravelReportName);
                _selection.SelectedTRUser = travelReports;
                return View(selection);
            }
            else if (User.IsInRole("WorkAdministrator"))
            {
               // var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                var lenaReports = db.PatientUsers.Where(t => t.StaffUserId == LenaUser).Include(t => t.Patient).Where(t => t.PatientId == t.Patient.PatientId && t.Patient.CustomerId == ActiveUser.CustomerId);
                var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == ActiveUser.Id).OrderBy(t => t.TravelReportName);
                //var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                //var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId && t.PatientId == t.Patient.PatientId).Include(t => t.PatientUsers).Where(r => r.StaffUserId == ActiveUser.Id)).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                // var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId && t.PatientId == t.Patient.PatientId).Include(t => t.PatientUsers.Select(r => r.StaffUserId == ActiveUser.Id)).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                // var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId && t.PatientId == t.Patient.PatientId).Include(t => t.PatientUsers.Where(r => r.StaffUserId == ActiveUser.Id)).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                //var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == ActiveUser.Id).OrderBy(t => t.TravelReportName);

                _selection.SelectedTRUser = travelReports;
                return View(selection);
                }
            else
                {
               
                    if (selection.UserList == null)
                    {
                        //var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                        var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                        _selection.SelectedTRUser = travelReports;
                        var _selectiont1 = new TravelReportViewModel();


                        _selectiont1.TravelUsers = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", ActiveUser.Id);

                        _selection.UserList = _selectiont1;

                        }
                        else
                        {
                        var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == selection.UserList.SelectedTravelUser).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                        _selection.SelectedTRUser = travelReports;
                        _selection.UserList.TravelUsers = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", ActiveUser.Id);
                         }
                        return View(_selection);

                }
            }
            //if (User.IsInRole("Workassistant"))  // försök att lösa "ta bort" "ingen kund" TR rapporter i listan.
            //{
            //    if (selection.UserList == null)
            //    {
            //        //var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
            //        var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
            //       // var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId).Include(t => t.PatientUser).Where(t => t.PatientId == t.Patient.PatientId).Select(g => g.PatientUsers).Where(p => p.StaffUserId == ApplicationUser.Id).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
            //        //var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId).Include(t => t.PatientUsers).Where(t => t.PatientId == t.Patient.PatientId).Include(g => g.PatientUsers).Where(t => t.PatientId == t.PatientUsers.PatientId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
            //        //ViewBag.PatientId = new SelectList(db.PatientUsers.Where(p => p.StaffUserId == ActiveUser.Id).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select(g => g.Patient), "PatientId", "PatientName");
            //        _selection.SelectedTRUser = travelReports;
            //        var _selectiont1 = new TravelReportViewModel();


            //        _selectiont1.TravelUsers = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", ActiveUser.Id);

            //        _selection.UserList = _selectiont1;

            //    }
            //    else
            //    {
            //        var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == selection.UserList.SelectedTravelUser).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
            //        _selection.SelectedTRUser = travelReports;
            //        _selection.UserList.TravelUsers = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", ActiveUser.Id);
            //    }


            //}
            //else
            //{
            //    if (selection.UserList == null)
            //    {
            //        //var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
            //        var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
            //        _selection.SelectedTRUser = travelReports;
            //        var _selectiont1 = new TravelReportViewModel();


            //        _selectiont1.TravelUsers = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", ActiveUser.Id);

            //        _selection.UserList = _selectiont1;

            //    }
            //    else
            //    {
            //        var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == selection.UserList.SelectedTravelUser).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
            //        _selection.SelectedTRUser = travelReports;
            //        _selection.UserList.TravelUsers = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", ActiveUser.Id);
            //    }
        


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


        // GET: TravelReports/CreateTest
        public ActionResult CreateTest()
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
            //var patientUser = db.PatientUsers.Where(pu => pu.StaffUserId == ActiveUser.Id).Include(p =>p.Patient);
            
            //ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.StatusName = db.StatusTypes.FirstOrDefault().StatusName;
            ViewBag.StatusTypeId1 = db.StatusTypes.Where(stt => stt.StatusName == "Ny").FirstOrDefault().StatusTypeId;
            ViewBag.ApplicationUserId1 = ActiveUser.Id;
            //var pats = db.Patients.Include(pu => pu.PatientUser).Where(p => p.PatientUser.StaffUserId == ActiveUser.Id);
            //ViewBag.PatientId = new SelectList(db.Patients.Where(p => p.PatientUser.StaffUserId == ActiveUser.Id), "PatientId", "PatientName", travelReport.PatientId);

            //ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "PatientName");

            ViewBag.PatientId = new SelectList(db.PatientUsers.Where(p => p.StaffUserId == ActiveUser.Id).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select(g => g.Patient), "PatientId", "PatientName");
            ViewBag.PatientId1 = new SelectList(db.Patients, "PatientId", "PatientName");
            //ViewBag.PatientId = new SelectList(db.PatientUsers.Where(p => p.StaffUserId == ActiveUser.Id).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select(g => g.Patient.PatientName), db.PatientUsers.Where(p => p.StaffUserId == ActiveUser.Id).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select(g => g.Patient.PatientId));

            //ViewBag.PatientId = new SelectList(db.PatientUsers.Where(p => p.StaffUserId == ActiveUser.Id).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select((f => new SelectListItem
            //                                   {
            //                                       Value = f.Patient.PatientId.ToString(),
            //                                       Text = f.Patient.PatientName
            //})));
            //ViewBag.PatientId = new SelectList(db.Users.Include(u => u.Patient) Where(p => p.StaffUserId == ActiveUser.Id).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select(g => g.Patient.PatientName), db.PatientUsers.Where(p => p.StaffUserId == ActiveUser.Id).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select(g => g.Patient.PatientId));


            return View();
        }

        // POST: TravelReports/CreateTest
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTest([Bind(Include = "TravelReportId,ApplicationUserId,PatientId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastDeduction,LunchOrDinnerDeduction,LunchAndDinnerDeduction,AllMealsDeduction,StatusTypeId,Comment")] TravelReport travelReport , Patient patient)
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

        // GET: TravelReports/Create
        public ActionResult Create()
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
            //ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.StatusName = db.StatusTypes.FirstOrDefault().StatusName;
            ViewBag.StatusTypeId1 = db.StatusTypes.Where(stt => stt.StatusName == "Ny").FirstOrDefault().StatusTypeId;
            ViewBag.ApplicationUserId1 = ActiveUser.Id;
     
            ViewBag.PatientId = new SelectList(db.PatientUsers.Where(p => p.StaffUserId == ActiveUser.Id).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select(g => g.Patient), "PatientId", "PatientName");
            //ViewBag.PatientId = new SelectList(db.PatientUsers.Where(p => p.StaffUserId == ActiveUser.Id).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select(g => g.Patient), "PatientId", "PatientName", String.Empty);

            return View();
        }

        // POST: TravelReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TravelReportId,ApplicationUserId,PatientId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastDeduction,LunchOrDinnerDeduction,LunchAndDinnerDeduction,AllMealsDeduction,StatusTypeId,Comment")] TravelReport travelReport)
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
            //if (PatientId == null)
            //{
            //    travelReport.PatientId = 0;
            //}
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
        public ActionResult Edit([Bind(Include = "TravelReportId,ApplicationUserId,PatientId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastDeduction,LunchOrDinnerDeduction,LunchAndDinnerDeduction,AllMealsDeduction,Status,Comment")] TravelReport travelReport)
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
            ViewBag.PatientId = new SelectList(db.PatientUsers.Where(p => p.StaffUserId == travelReport.ApplicationUserId).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select(g => g.Patient), "PatientId", "PatientName", travelReport.PatientId);
            return View(travelReport);
        }

        // POST: TravelReports1/Edit1/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit1([Bind(Include = "TravelReportId,ApplicationUserId,PatientId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastDeduction,LunchOrDinnerDeduction,LunchAndDinnerDeduction,AllMealsDeduction,StatusTypeId,Comment")] TravelReport travelReport)
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
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
            ViewBag.ActiveUser = ActiveUser.Id;

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
        public ActionResult Calc([Bind(Include = "TravelReportId,ApplicationUserId,PatientId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastDeduction,LunchOrDinnerDeduction,LunchAndDinnerDeduction,AllMealsDeduction,StatusTypeId,Comment")] TravelReport travelReport, string button)
        {
            if (ModelState.IsValid)
            {

                if (button == "Ändra traktamente")
                {
                    //travelReport.StatusTypeId = db.StatusTypes.Where(stt => stt.StatusName == "Ny/Beräknad").FirstOrDefault().StatusTypeId;
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Edit2", new { id = travelReport.TravelReportId });
                }
                if (button == "Ny resekostnad")
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
                if (button == "Godkänd")
                {
                    travelReport.StatusTypeId = db.StatusTypes.Where(stt => stt.StatusName == "Godkänd").FirstOrDefault().StatusTypeId;
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                if (button == "Ej godkänd")
                {
                    travelReport.StatusTypeId = db.StatusTypes.Where(stt => stt.StatusName == "Ej godkänd").FirstOrDefault().StatusTypeId;
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                if (button == "Till listan")
                {
                    return RedirectToAction("Index");
                }
                if (button == "Ändra")
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
        public ActionResult Edit2([Bind(Include = "TravelReportId,ApplicationUserId,PatientId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastDeduction,LunchOrDinnerDeduction,LunchAndDinnerDeduction,AllMealsDeduction,StatusTypeId,Comment")] TravelReport travelReport, string button)
        {
            if (ModelState.IsValid)
            {

                if (button == "Visa/ändra")
                {
                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Calc", new { id = travelReport.TravelReportId });
                }
                if (button == "Ny resekostnad")
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
                if (button == "Ändra")
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
