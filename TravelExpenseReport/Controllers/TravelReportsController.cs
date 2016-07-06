﻿using System;
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
        private ApplicationDbContext db = new ApplicationDbContext();

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

        public void SaveNote(TravelReport travelReport)
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();

            Note note = new Note();
            note.ApplicationUserId = ActiveUser.FullName;
            note.NoteTime = DateTime.Now;
            note.NoteStatus = db.StatusTypes.Where(stt => stt.StatusTypeId == travelReport.StatusTypeId).FirstOrDefault().StatusName;
            note.NoteInfo = travelReport.Comment;
            note.TravelReportId = travelReport.TravelReportId;
            db.Notes.Add(note);
            db.SaveChanges();
        }

        public string Last2Notes(int travelReportId)
        {
            var notesToSearch = db.Notes.Where(e => e.TravelReportId == travelReportId).OrderByDescending(n => n.NoteTime);
            int i = 0;
            string x1 = "";
            string x2 = "";
            string x3 = "";
            foreach (var x in notesToSearch)
            {
                if (i == 0)
                {
                    x1 = x.NoteTime.Date.ToString("d") + " " + x.NoteStatus + " " + x.ApplicationUserId;
                    i++;
                }
                else if (i == 1)
                {
                    x2 = x.NoteTime.Date.ToString("d") + " " + x.NoteStatus + " " + x.ApplicationUserId;
                    break;
                }
            }
            x3 = x1 + " , " + x2;
            return (x3);

        }

        public List<TravelReport> AllowedTRList(IOrderedQueryable<TravelReport> TRReports, List<PatientUser> patientsForUser)
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();

            List<TravelReport> travelReports = new List<TravelReport>();

            foreach (var tr in TRReports)
            {
                if (ActiveUser.Id == tr.ApplicationUserId)
                {
                    travelReports.Add(tr);
                }
                else
                {
                    foreach (var p in patientsForUser)
                    {
                        if (tr.PatientId == p.PatientId)
                        {
                            travelReports.Add(tr);
                        }

                    }
                }
            }
            return travelReports;
        }

        public List<ApplicationUser> AllowedTRUserList(List<TravelReport> travelReports)
        {
            List<ApplicationUser> allowedTRUsers = new List<ApplicationUser>();

            foreach (var tr in travelReports)
            {
                ApplicationUser allowedTRUser = db.Users.Find(tr.ApplicationUserId);
                if (!allowedTRUsers.Contains(allowedTRUser))
                {
                    allowedTRUsers.Add(allowedTRUser);
                }
            }
            return allowedTRUsers;

        }

        public ActionResult Index(SelectionAndTRViewModel selection, string selectedUserId)
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
            ViewBag.ActiveUser = ActiveUser.Id;

            var _selection = selection;

            if (User.IsInRole("Assistant"))
            {
                var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == ActiveUser.Id).OrderBy(t => t.TravelReportName);
                _selection.SelectedUserTravelReports = travelReports;
                return View(selection);
            }
            else if (User.IsInRole("Patient"))
            {
                var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.PatientId == ActiveUser.PatientId).OrderBy(t => t.TravelReportName);
                _selection.SelectedUserTravelReports = travelReports;
                return View(selection);
            }
            else if (User.IsInRole("Other"))
            {
                var patientsForUser = db.PatientUsers.Where(t => t.StaffUserId == ActiveUser.Id).Include(t => t.Patient).Where(t => t.PatientId == t.Patient.PatientId && t.Patient.CustomerId == ActiveUser.CustomerId).ToList();
                var TRReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                _selection.SelectedUserTravelReports = AllowedTRList(TRReports, patientsForUser);
                return View(selection);
            }
            else if (User.IsInRole("GroupAdmin"))
            {
                ViewBag.Filtered = true;
                var _selectionTravelUser = new SelectTravelUserViewModel();
                var patientsForUser = db.PatientUsers.Where(t => t.StaffUserId == ActiveUser.Id).Include(t => t.Patient).Where(t => t.PatientId == t.Patient.PatientId && t.Patient.CustomerId == ActiveUser.CustomerId).ToList();
                if (selection.SelectionList == null)
                {
                    if (selectedUserId == null)
                    {
                        var TRReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                        var travelReports = AllowedTRList(TRReports, patientsForUser);
                        _selection.SelectedUserTravelReports = travelReports;
                        //var _selectiontTravelUser = new SelectTravelUserViewModel();
                        _selectionTravelUser.TravelUsersForSelection = new SelectList(AllowedTRUserList(travelReports).OrderBy(t => t.FullName), "Id", "FullName", ActiveUser.Id);
                        _selection.SelectionList = _selectionTravelUser;
                        ViewBag.Filtered = false;
                    }
                    else // Selected to keep when return to list
                    {
                        var TRReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == selectedUserId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                        var travelReports = AllowedTRList(TRReports, patientsForUser);

                        _selection.SelectedUserTravelReports = travelReports;
                        //var _selectiont1 = new SelectTravelUserViewModel();
                        _selectionTravelUser.TravelUsersForSelection = new SelectList(AllowedTRUserList(travelReports), "Id", "FullName", ActiveUser.Id);
                        _selectionTravelUser.SelectedTravelUser = selectedUserId;
                        _selection.SelectionList = _selectionTravelUser;
                        ViewBag.Filtered = true;
                    }
                }
                else // Returns here when filtering
                {
                    if (selectedUserId == null)
                    {
                        var TRReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == selection.SelectionList.SelectedTravelUser).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                        var travelReports = AllowedTRList(TRReports, patientsForUser);
                        _selection.SelectedUserTravelReports = travelReports;
                        _selection.SelectionList.TravelUsersForSelection = new SelectList(AllowedTRUserList(travelReports), "Id", "FullName", ActiveUser.Id);
                        ViewBag.Filtered = true;
                    }
                    else // när kommer vi hit?
                    {
                        var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == selectedUserId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                        _selection.SelectedUserTravelReports = travelReports;
                        //var _selectiont1 = new SelectTravelUserViewModel();
                        _selectionTravelUser.TravelUsersForSelection = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", selectedUserId);
                        _selection.SelectionList = _selectionTravelUser;
                        ViewBag.Filtered = true;
                    }
                }
                return View(_selection);
            }
            else if (User.IsInRole("Administrator"))
            {
                var _selectionTravelUser = new SelectTravelUserViewModel();
                ViewBag.Filtered = true;
                if (selection.SelectionList == null)
                {
                    if (selectedUserId == null)
                    {
                        var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUser.CustomerId == ActiveUser.CustomerId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                        _selection.SelectedUserTravelReports = travelReports;
                        //var _selectiont1 = new SelectTravelUserViewModel();
                        _selectionTravelUser.TravelUsersForSelection = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", ActiveUser.Id);
                        _selection.SelectionList = _selectionTravelUser;
                        ViewBag.Filtered = false;
                    }
                    else
                    {
                        var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == selectedUserId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                        _selection.SelectedUserTravelReports = travelReports;
                        //var _selectiont1 = new SelectTravelUserViewModel();
                        _selectionTravelUser.TravelUsersForSelection = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", selectedUserId);
                        _selectionTravelUser.SelectedTravelUser = selectedUserId;
                        _selection.SelectionList = _selectionTravelUser;
                    }
                }
                else
                {
                    if (selectedUserId == null)
                    {
                        var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == selection.SelectionList.SelectedTravelUser).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                        _selection.SelectedUserTravelReports = travelReports;
                        _selection.SelectionList.TravelUsersForSelection = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", ActiveUser.Id);
                        ViewBag.SelectedUserId = _selection.SelectionList.SelectedTravelUser;
                    }
                    else
                    {
                        var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == selectedUserId).OrderBy(t => t.ApplicationUser.FullName).ThenBy(t => t.TravelReportName);
                        _selection.SelectedUserTravelReports = travelReports;
                        //var _selectiont1 = new SelectTravelUserViewModel();
                        _selectionTravelUser.TravelUsersForSelection = new SelectList(db.Users.Where(t => t.CustomerId == ActiveUser.CustomerId && t.PatientId == 0), "Id", "FullName", selectedUserId);
                        _selection.SelectionList = _selectionTravelUser;
                    }
                }
                return View(_selection);
            }
            else
            {
                var travelReports = db.TravelReports.Include(t => t.ApplicationUser).Include(t => t.StatusType).Include(t => t.Patient).Where(t => t.ApplicationUserId == ActiveUser.Id).OrderBy(t => t.TravelReportName);
                _selection.SelectedUserTravelReports = travelReports;
                return View(_selection);
            }
        }


        // GET: TravelReports/Details/5
        public ActionResult Details(int? id, string selectedUserId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.SelectedUserId = selectedUserId;

            TravelReport travelReport = db.TravelReports.Find(id);
            if (travelReport == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Assistant"))
            {
                var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
                if (travelReport.ApplicationUserId != ActiveUser.Id)
                {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            
            ViewBag.Traktamente = (travelReport.Night != 0);
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

            return View(travelReport);
        }

        // GET: TravelReports/Details/5
        public ActionResult Print(int? id, string selectedUserId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.SelectedUserId = selectedUserId;

            TravelReport travelReport = db.TravelReports.Find(id);
            if (travelReport == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Assistant"))
            {
                var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
                if (travelReport.ApplicationUserId != ActiveUser.Id)
                {
                    return HttpNotFound();
                }
            }

            ViewBag.Traktamente = (travelReport.Night != 0);
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
        //public ActionResult Create([Bind(Include = "TravelReportId,ApplicationUserId,PatientId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,StatusTypeId,Comment")] TravelReport travelReport)
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
            ViewBag.PatientId = new SelectList(db.PatientUsers.Where(p => p.StaffUserId == ActiveUser.Id).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select(g => g.Patient), "PatientId", "PatientName");

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
            //ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName", travelReport.ApplicationUserId);
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
            //ViewBag.StatusTypeId = new SelectList(db.StatusTypes, "StatusTypeId", "StatusName", travelReport.StatusTypeId);
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

            if (User.IsInRole("Assistant"))
            {
                var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
                if (travelReport.ApplicationUserId != ActiveUser.Id)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "FullName", travelReport.ApplicationUserId);
            ViewBag.StatusName = db.StatusTypes.Find(travelReport.StatusTypeId).StatusName;
            //ViewBag.StatusTypeId = new SelectList(db.StatusTypes, "StatusTypeId", "StatusName", travelReport.StatusTypeId);
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
            ViewBag.StatusName = db.StatusTypes.Find(travelReport.StatusTypeId).StatusName;
            ViewBag.PatientId = new SelectList(db.PatientUsers.Where(p => p.StaffUserId == travelReport.ApplicationUserId).Include(g => g.Patient).Where(g => g.PatientId == g.Patient.PatientId).Select(g => g.Patient), "PatientId", "PatientName", travelReport.PatientId);
            return View(travelReport);
        }

        // GET: TravelReports/Calc/5
        //
        // The view Calculate will present the calculated sum of allowance for expenses
        //
        public ActionResult Calc(int? id, string selectedUserId)
        {
            var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
            ViewBag.ActiveUser = ActiveUser.Id;
            ViewBag.SelectedUserId = selectedUserId;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            TravelReport travelReport = db.TravelReports.Find(id);
            if (travelReport == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Assistant"))
            {
                if (travelReport.ApplicationUserId != ActiveUser.Id)
                {
                    return HttpNotFound();
                }
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

            ViewBag.Comment = travelReport.Comment;

            travelReport.Comment = "";

            return View(travelReport);
        }


        // POST: TravelReports/Calc/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Calc([Bind(Include = "TravelReportId,ApplicationUserId,PatientId,TravelReportName,Destination,Purpose,DepartureDate,DepartureTime,ReturnDate,ReturnTime,DepartureHoursExtra,ReturnHoursExtra,FullDay,HalfDay,Night,BreakfastDeduction,LunchOrDinnerDeduction,LunchAndDinnerDeduction,AllMealsDeduction,StatusTypeId,Comment")] TravelReport travelReport, string button, string selectedUserId)
        {
            if (ModelState.IsValid)
            {
                var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();

                if (button == "Ändra traktamente")
                {
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

                    SaveNote(travelReport);

                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { selectedUserId = selectedUserId });
                }
                if (button == "Godkänd")
                {
                    travelReport.StatusTypeId = db.StatusTypes.Where(stt => stt.StatusName == "Godkänd").FirstOrDefault().StatusTypeId;

                    SaveNote(travelReport);

                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { selectedUserId = selectedUserId });
                }
                if (button == "Ej godkänd")
                {
                    travelReport.StatusTypeId = db.StatusTypes.Where(stt => stt.StatusName == "Ej godkänd").FirstOrDefault().StatusTypeId;

                    SaveNote(travelReport);

                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { selectedUserId = selectedUserId });
                }
                if (button == "Verifierad")
                {
                    travelReport.StatusTypeId = db.StatusTypes.Where(stt => stt.StatusName == "Verifierad").FirstOrDefault().StatusTypeId;

                    SaveNote(travelReport);

                    travelReport.Comment = Last2Notes(travelReport.TravelReportId);

                    db.Entry(travelReport).State = EntityState.Modified;
                    db.SaveChanges();
                    DeleteNotes(travelReport.TravelReportId);

                    return RedirectToAction("Index", new { selectedUserId = selectedUserId });
                }
                if (button == "Till listan")
                {
                    return RedirectToAction("Index", new { selectedUserId = selectedUserId });
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
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (travelReport == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Assistant"))
            {
                if (travelReport.ApplicationUserId != ActiveUser.Id)
                {
                    return HttpNotFound();
                }
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

                if (button == "Gör klar")
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



        //Delete Expenses for TravelReport
        public void DeleteExpenses(int travelReportId)
        {
            int actualTravelreportId = travelReportId;

            var expensesToDelete = db.Expenses.Where(e => e.TravelReportId == actualTravelreportId);

            foreach (var ex in expensesToDelete)
            {
                Expense expense = db.Expenses.Find(ex.ExpenseId);
                db.Expenses.Remove(expense);

            }
            db.SaveChanges();

        }

        //Delete Notes for TravelReport
        public void DeleteNotes(int travelReportId)
        {
            int actualTravelreportId = travelReportId;

            var notesToDelete = db.Notes.Where(e => e.TravelReportId == actualTravelreportId);

            foreach (var n in notesToDelete)
            {
                Note note = db.Notes.Find(n.NoteId);
                db.Notes.Remove(note);
            }
            db.SaveChanges();
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
                        if (User.IsInRole("Assistant"))
            {
                var ActiveUser = db.Users.Where(u => u.UserName == User.Identity.Name.ToString()).ToList().FirstOrDefault();
                if (travelReport.ApplicationUserId != ActiveUser.Id)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            return View(travelReport);
        }

        // POST: TravelReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //TravelReport travelReport = db.TravelReports.Find(id);
            //check for expenses
            var checkForExpense = db.Expenses.Where(e => e.TravelReportId == id).FirstOrDefault();
            if (checkForExpense != null)
            {
                DeleteExpenses(id);
            }
            //check for notes
            var checkForNote = db.Notes.Where(e => e.TravelReportId == id).FirstOrDefault();
            if (checkForNote != null)
            {
                DeleteNotes(id);
            }

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
