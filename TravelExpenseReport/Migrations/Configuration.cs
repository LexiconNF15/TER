namespace TravelExpenseReport.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TravelExpenseReport.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TravelExpenseReport.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            foreach (string roleName in new[] { "Assistant", "WorkAdministrator", "Patient", "Administrator", "SysAdmin" })
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    var role = new IdentityRole { Name = roleName };
                    roleManager.Create(role);
                }

            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var NewUserList = new List<ApplicationUser>();

            var users = new List<ApplicationUser> {
                new ApplicationUser {FullName = "Oscar Antonsson", Email = "oscar.antonsson@ab.se", UserName = "oscar.antonsson@ab.se",CustomerId = 1},
                new ApplicationUser {FullName = "Sara Björn", Email = "sara.bjorn@test.se", UserName = "sara.bjorn@test.se",CustomerId = 2},
                new ApplicationUser {FullName = "Allan Persson", Email = "allan.persson@ab.se", UserName = "allan.persson@ab.se",CustomerId = 1}

            };

            foreach (var u in users)
            {
                userManager.Create(u, "foobar");
                var user = userManager.FindByEmail(u.Email);
                NewUserList.Add(user);
                userManager.AddToRole(user.Id, "WorkAdministrator");
            }


            var users2 = new List<ApplicationUser> {
                new ApplicationUser {FullName = "Lena Källgren", Email = "lena.kallgren@ab.se", UserName = "lena.kallgren@ab.se", CustomerId = 1},
                new ApplicationUser {FullName = "Bella Ax", Email = "bella.ax@test.se", UserName = "bella.ax@test.se", CustomerId = 2},
                new ApplicationUser {FullName = "Rickard Nilsson", Email = "rickard.nilsson@ab.se", UserName = "rickard.nilsson@ab.se", CustomerId = 1}
            };

            foreach (var u in users2)
            {
                userManager.Create(u, "foobar");
                var user = userManager.FindByEmail(u.Email);
                NewUserList.Add(user);
                userManager.AddToRole(user.Id, "Assistant");
            }

            var users3 = new List<ApplicationUser> {
                new ApplicationUser {FullName = "Nicklas Sten", Email = "nicklas.sten@ab.se", UserName = "nicklas.sten@ab.se", CustomerId = 1, PatientId = 1},
                new ApplicationUser {FullName = "Carmen Sanchez", Email = "carmen.sanchez@test.se", UserName = "carmen.sanchez@test.se", CustomerId = 2, PatientId = 2},
                new ApplicationUser {FullName = "Diana Westman", Email = "diana.westman@test.se", UserName = "diana.westma@test.se", CustomerId = 2, PatientId = 3},
                new ApplicationUser {FullName = "Eros Venti", Email = "eros.venti@ab.se", UserName = "eros.venti@ab.se", CustomerId = 1, PatientId = 4}
            };

            foreach (var u in users3)
            {
                userManager.Create(u, "foobar");
                var user = userManager.FindByEmail(u.Email);
                NewUserList.Add(user);
                userManager.AddToRole(user.Id, "Patient");
            }

            var users4 = new List<ApplicationUser> {
                new ApplicationUser {FullName = "Anna Karlsson", Email = "anna.karlsson@ab.se", UserName = "anna.karlsson@ab.se",CustomerId = 1},
                new ApplicationUser {FullName = "Ulf Svensson", Email = "ulf.svensson@test.se", UserName = "ulf.svensson@test.se",CustomerId = 2},
                new ApplicationUser {FullName = "Paula Abdul", Email = "paula.abdul@ab.se", UserName = "paula.abdul@ab.se",CustomerId = 1}
             };

            foreach (var u in users4)
            {
                userManager.Create(u, "foobar");
                var user = userManager.FindByEmail(u.Email);
                NewUserList.Add(user);
                userManager.AddToRole(user.Id, "Administrator");
            }
            var users5 = new List<ApplicationUser> {
                new ApplicationUser {FullName = "Cecilia Ritter", Email = "cecilia.ritter@ab.se", UserName = "cecilia.ritter@ab.se",CustomerId = 1},
                new ApplicationUser {FullName = "Cecilia Ritter", Email = "cecilia.ritter@test.se", UserName = "cecilia.ritter@test.se",CustomerId = 2}
            };

            foreach (var u in users5)
            {
                userManager.Create(u, "foobar");
                var user = userManager.FindByEmail(u.Email);
                NewUserList.Add(user);
                userManager.AddToRole(user.Id, "SysAdmin");
            }

            var expenseTypes = new List<ExpenseType> {
                new ExpenseType
                {
                    ExpenseTypeId = 1,
                    ExpenseTypeName = "Tåg"
                },
                new ExpenseType
                {
                    ExpenseTypeId = 2,
                    ExpenseTypeName = "Flyg"
                },
                 new ExpenseType
                {
                    ExpenseTypeId = 3,
                    ExpenseTypeName = "Taxi"
                },
                  new ExpenseType
                {
                    ExpenseTypeId = 4,
                    ExpenseTypeName = "Egen bil"
                },
                   new ExpenseType
                {
                    ExpenseTypeId = 5,
                    ExpenseTypeName = "Buss, spårvagn, mm"
                },
                   new ExpenseType
                    {
                    ExpenseTypeId = 6,
                    ExpenseTypeName = "Hyrbil"
                },
                   new ExpenseType
                    {
                    ExpenseTypeId = 7,
                    ExpenseTypeName = "Övrigt"
                }
               };

            foreach (var et in expenseTypes)
            {
                context.ExpenseTypes.AddOrUpdate(e => e.ExpenseTypeName, et);
                //context.ExpenseTypes.AddOrUpdate(et);

            }

            var statusTypes = new List<StatusType> {
                new StatusType
                {
                    StatusTypeId = 1,
                    StatusName = "Ny"
                },
                new StatusType
                {
                    StatusTypeId = 2,
                    StatusName = "Inskickad"
                },
                new StatusType
                {
                    StatusTypeId = 3,
                    StatusName = "Ej godkänd"
                },
                new StatusType
                {
                    StatusTypeId = 4,
                    StatusName = "Godkänd"
                },
                new StatusType
                {
                    StatusTypeId = 5,
                    StatusName = "För utbetalning"
                },
                new StatusType
                {
                    StatusTypeId = 6,
                    StatusName = "Utbetald"
                    }
                //new StatusType
                //{
                //    StatusTypeId = 7,
                //    StatusName = "Ny/Beräknad"
                //    },
                //new StatusType
                //{
                //    StatusTypeId = 8,
                //    StatusName = "Ny/Summerad"
                //}
            };

            foreach (var st in statusTypes)
            {
                context.StatusTypes.AddOrUpdate(s => s.StatusName, st);

            }

          
            var legalAmount = new List<LegalAmount> {
                new LegalAmount
                {
                    LegalAmountId = 1,
                    ValidDate = DateTime.Parse("2014-01-01"),
                    FullDayAmount = 210,
                    HalfDayAmount = 105,
                    NightAmount = 100,
                    MilageAmount = 175,
                    BreakfastAmount = 42,
                    LunchOrDinnerAmount = 74,
                    LunchAndDinnerAmount =148,
                    AllMealsAmount = 190

                },
                new LegalAmount
                {
                   LegalAmountId = 2,
                    ValidDate = DateTime.Parse("2015-01-01"),
                    FullDayAmount = 220,
                    HalfDayAmount = 110,
                    NightAmount = 110,
                    MilageAmount = 185,
                    BreakfastAmount = 44,
                    LunchOrDinnerAmount = 77,
                    LunchAndDinnerAmount =154,
                    AllMealsAmount = 198
                }
            };

            foreach (var la in legalAmount)
            {
                context.LegalAmounts.AddOrUpdate(l => l.LegalAmountId, la);
                //context.ExpenseTypes.AddOrUpdate(et);

            }

            var travelReport = new List<TravelReport> {
                new TravelReport
                {
                    TravelReportId = 1,
                   // ApplicationUserId = "7bce74df-983d-4b45-81d5-530634133665",
                    ApplicationUserId = NewUserList[2].Id,
                    PatientId = 4,
                    TravelReportName = "2016-001",
                    Destination = "Flen",
                    Purpose = "Utbildning",
                    DepartureDate = DateTime.Parse("2016-04-20 00:00:00"),
                    DepartureTime = TimeSpan.Parse("13:00:00"),
                    ReturnDate = DateTime.Parse("2016-05-22 00:00:00"),
                    ReturnTime = TimeSpan.Parse("16:00:00"),
                    DepartureHoursExtra = 1 ,
                    ReturnHoursExtra = 2,
                    FullDay = 33,
                    HalfDay = 2,
                    Night = 33,
                    BreakfastDeduction = 0,
                    LunchOrDinnerDeduction = 0,
                    LunchAndDinnerDeduction = 0,
                    AllMealsDeduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 2,
                    //ApplicationUserId = "7bce74df-983d-4b45-81d5-530634133665",
                    ApplicationUserId = NewUserList[1].Id,
                    PatientId = 3,
                    TravelReportName = "2016-001",
                    Destination = "Malmö",
                    Purpose = "Studiebesök",
                    DepartureDate = DateTime.Parse("2016-04-23 00:00:00"),
                    DepartureTime = TimeSpan.Parse("08:30:00"),
                    ReturnDate = DateTime.Parse("2016-04-27 00:00:00"),
                    ReturnTime = TimeSpan.Parse("20:00:00"),
                    DepartureHoursExtra = 1 ,
                    ReturnHoursExtra = 2,
                    FullDay = 4,
                    HalfDay = 0,
                    Night = 5,
                    BreakfastDeduction = 0,
                    LunchOrDinnerDeduction = 1,
                    LunchAndDinnerDeduction = 0,
                    AllMealsDeduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 3,
                    //ApplicationUserId = "855555ef-8f46-4281-9161-5777699b4d2d",
                    ApplicationUserId = NewUserList[3].Id,
                    PatientId = 1,
                    TravelReportName = "2016-001",
                    Destination = "Uppsala",
                    Purpose = "Läger",
                    DepartureDate = DateTime.Parse("2016-04-20 00:00:00"),
                    DepartureTime = TimeSpan.Parse("17:45:00"),
                    ReturnDate = DateTime.Parse("2016-04-25 00:00:00"),
                    ReturnTime = TimeSpan.Parse("07:30:00"),
                    DepartureHoursExtra = 2 ,
                    ReturnHoursExtra = 0,
                    FullDay = 4,
                    HalfDay = 2,
                    Night = 5,
                    BreakfastDeduction = 0,
                    LunchOrDinnerDeduction = 0,
                    LunchAndDinnerDeduction = 1,
                    AllMealsDeduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 4,
                    //ApplicationUserId = "855555ef-8f46-4281-9161-5777699b4d2d",
                    ApplicationUserId = NewUserList[2].Id,
                    PatientId = 1,
                    TravelReportName = "2015-001",
                    Destination = "Sundsvall",
                    Purpose = "Besöka släkt över Valborg",
                    DepartureDate = DateTime.Parse("2015-06-29 00:00:00"),
                    DepartureTime = TimeSpan.Parse("11:59:00"),
                    ReturnDate = DateTime.Parse("2015-08-01 00:00:00"),
                    ReturnTime = TimeSpan.Parse("18:30:00"),
                    DepartureHoursExtra = 2 ,
                    ReturnHoursExtra = 0,
                    FullDay = 31,
                    HalfDay = 2,
                    Night = 32,
                    BreakfastDeduction = 0,
                    LunchOrDinnerDeduction = 0,
                    LunchAndDinnerDeduction = 0,
                    AllMealsDeduction = 10,
                    StatusTypeId = 1,
                    Comment = null
                 },
                 new TravelReport
                {
                    TravelReportId = 5,
                    //ApplicationUserId = "cb791d4e-92a8-41ba-aeb3-be2d3000af15",
                    ApplicationUserId = NewUserList[5].Id,
                    PatientId = 4,
                    TravelReportName = "2016-001",
                    Destination = "Göteborg",
                    Purpose = "Besök på Liseberg",
                    DepartureDate = DateTime.Parse("2016-04-24 00:00:00"),
                    DepartureTime = TimeSpan.Parse("06:00:00"),
                    ReturnDate = DateTime.Parse("2016-04-27 00:00:00"),
                    ReturnTime = TimeSpan.Parse("17:00:00"),
                    DepartureHoursExtra = 0 ,
                    ReturnHoursExtra = 2,
                    FullDay = 3,
                    HalfDay = 1,
                    Night = 3,
                    BreakfastDeduction = 0,
                    LunchOrDinnerDeduction = 0,
                    LunchAndDinnerDeduction = 0,
                    AllMealsDeduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 6,
                    //ApplicationUserId = "cb791d4e-92a8-41ba-aeb3-be2d3000af15",
                    ApplicationUserId = NewUserList[1].Id,
                    TravelReportName = "2016-002",
                    PatientId = 3,
                    Destination = "Västerås",
                    Purpose = "Bandymatch",
                    DepartureDate = DateTime.Parse("2016-04-19 00:00:00"),
                    DepartureTime = TimeSpan.Parse("08:30:00"),
                    ReturnDate = DateTime.Parse("2016-05-29 00:00:00"),
                    ReturnTime = TimeSpan.Parse("18:30:00"),
                    DepartureHoursExtra = 1 ,
                    ReturnHoursExtra = 2,
                    FullDay = 40,
                    HalfDay = 1,
                    Night = 40,
                    BreakfastDeduction = 0,
                    LunchOrDinnerDeduction = 1,
                    LunchAndDinnerDeduction = 0,
                    AllMealsDeduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 7,
                    //ApplicationUserId = "f77513f6-4c8b-4eb2-9896-b292dd9a294e",
                    ApplicationUserId = NewUserList[3].Id,
                    PatientId = 4,
                    TravelReportName = "2016-002",
                    Destination = "Enköping",
                    Purpose = "Studiebesök boende",
                    DepartureDate = DateTime.Parse("2016-04-26 00:00:00"),
                    DepartureTime = TimeSpan.Parse("17:45:00"),
                    ReturnDate = DateTime.Parse("2016-04-29 00:00:00"),
                    ReturnTime = TimeSpan.Parse("07:30:00"),
                    DepartureHoursExtra = 2 ,
                    ReturnHoursExtra = 0,
                    FullDay = 2,
                    HalfDay = 2,
                    Night = 3,
                    BreakfastDeduction = 0,
                    LunchOrDinnerDeduction = 0,
                    LunchAndDinnerDeduction = 0,
                    AllMealsDeduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 8,
                    //ApplicationUserId = "f77513f6-4c8b-4eb2-9896-b292dd9a294e",
                    ApplicationUserId = NewUserList[4].Id,
                    PatientId = 2,
                    TravelReportName = "2016-001",
                    Destination = "Sundsvall",
                    Purpose = "Besöka släkt",
                    DepartureDate = DateTime.Parse("2016-05-02 00:00:00"),
                    DepartureTime = TimeSpan.Parse("10:30:00"),
                    ReturnDate = DateTime.Parse("2016-06-07 00:00:00"),
                    ReturnTime = TimeSpan.Parse("19:30:00"),
                    DepartureHoursExtra = 0,
                    ReturnHoursExtra = 2,
                    FullDay = 37,
                    HalfDay = 0,
                    Night = 36,
                    BreakfastDeduction = 0,
                    LunchOrDinnerDeduction = 0,
                    LunchAndDinnerDeduction = 0,
                    AllMealsDeduction = 0,
                    StatusTypeId = 2,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 9,
                    //ApplicationUserId = "cb791d4e-92a8-41ba-aeb3-be2d3000af15",
                    ApplicationUserId = NewUserList[10].Id,
                    TravelReportName = "2016-001",
                    PatientId = 1,
                    Destination = "Sälen",
                    Purpose = "Konferens",
                    DepartureDate = DateTime.Parse("2016-07-01 00:00:00"),
                    DepartureTime = TimeSpan.Parse("08:45:00"),
                    ReturnDate = DateTime.Parse("2016-07-13 00:00:00"),
                    ReturnTime = TimeSpan.Parse("22:30:00"),
                    DepartureHoursExtra = 1 ,
                    ReturnHoursExtra = 2,
                    FullDay = 13,
                    HalfDay = 0,
                    Night = 13,
                    BreakfastDeduction = 0,
                    LunchOrDinnerDeduction = 1,
                    LunchAndDinnerDeduction = 0,
                    AllMealsDeduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 10,
                    //ApplicationUserId = "f77513f6-4c8b-4eb2-9896-b292dd9a294e",
                    ApplicationUserId = NewUserList[11].Id,
                    PatientId = 2,
                    TravelReportName = "2016-001",
                    Destination = "Malmö",
                    Purpose = "Ledningsgruppsmöte",
                    DepartureDate = DateTime.Parse("2016-07-14 00:00:00"),
                    DepartureTime = TimeSpan.Parse("17:45:00"),
                    ReturnDate = DateTime.Parse("2016-07-16 00:00:00"),
                    ReturnTime = TimeSpan.Parse("19:30:00"),
                    DepartureHoursExtra = 2 ,
                    ReturnHoursExtra = 0,
                    FullDay = 1,
                    HalfDay = 1,
                    Night = 1,
                    BreakfastDeduction = 0,
                    LunchOrDinnerDeduction = 0,
                    LunchAndDinnerDeduction = 0,
                    AllMealsDeduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 }
                };


            foreach (var tr in travelReport)
            {
                context.TravelReports.AddOrUpdate(t => t.TravelReportId, tr);
               
            }


            //var expenses = new List<Expense> {
            //                new Expense
            //                {
            //                    ExpenseId = 1,
            //                    ExpenseTypeId = 1,
            //                    ExpenseDescription = null,
            //                    ExpenseDate = DateTime.Parse("2016-04-20"),
            //                    ExpenseAmountInfo = "140,15",
            //                    ExpenseAmount = 140,
            //                    ExpenseMilage = 0,
            //                    TravelReportId = 1
            //                },
            //                 new Expense
            //                {
            //                    ExpenseId = 2,
            //                    ExpenseTypeId = 3,
            //                    ExpenseDescription = null,
            //                    ExpenseDate = DateTime.Parse("2016-04-20"),
            //                    ExpenseAmountInfo = "345,45",
            //                    ExpenseAmount = 345,
            //                    ExpenseMilage = 0,
            //                    TravelReportId = 1
            //                },
            //                  new Expense
            //                {
            //                    ExpenseId = 3,
            //                    ExpenseTypeId = 2,
            //                    ExpenseDescription = null,
            //                    ExpenseDate = DateTime.Parse("2016-04-23"),
            //                    ExpenseAmountInfo = "350,45",
            //                    ExpenseAmount = 350,
            //                    ExpenseMilage = 0,
            //                    TravelReportId = 2
            //                },
            //                 new Expense
            //                {
            //                    ExpenseId = 4,
            //                    ExpenseTypeId= 2,
            //                    ExpenseDescription = null,
            //                    ExpenseDate = DateTime.Parse("2016-04-27"),
            //                    ExpenseAmountInfo = "3100,00",
            //                    ExpenseAmount = 3100,
            //                    ExpenseMilage = 0,
            //                    TravelReportId = 5
            //                },
            //                new Expense
            //                {
            //                    ExpenseId = 5,
            //                    ExpenseTypeId = 4,
            //                    ExpenseDescription = null,
            //                    ExpenseDate = DateTime.Parse("2016-04-25"),
            //                    ExpenseAmountInfo = "485,95",
            //                    ExpenseAmount = 0,
            //                    ExpenseMilage = 485,
            //                    TravelReportId = 5
            //                },
            //                 new Expense
            //                {
            //                    ExpenseId = 6,
            //                    ExpenseTypeId = 4,
            //                    ExpenseDescription  = null,
            //                    ExpenseDate = DateTime.Parse("2016-04-27"),
            //                    ExpenseAmountInfo = "766,98",
            //                    ExpenseAmount = 0,
            //                    ExpenseMilage = 375,
            //                    TravelReportId = 6
            //                },
            //                 new Expense
            //                {
            //                     ExpenseId = 7,
            //                     ExpenseTypeId = 2,
            //                     ExpenseDescription = null,
            //                     ExpenseDate = DateTime.Parse("2016-05-09"),
            //                     ExpenseAmountInfo = "5630,00",
            //                     ExpenseAmount = 5630,
            //                     ExpenseMilage = 0,
            //                    TravelReportId = 8
            //                },
            //                new Expense
            //                {
            //                    ExpenseId = 8,
            //                    ExpenseTypeId = 3,
            //                    ExpenseDescription = null,
            //                    ExpenseDate = DateTime.Parse("2016-05-07"),
            //                    ExpenseAmountInfo = "367,50",
            //                    ExpenseAmount = 367,
            //                    ExpenseMilage = 0,
            //                    TravelReportId = 8
            //                 }
            //};

            //foreach (var ex in expenses)
            //{
            //    context.Expenses.AddOrUpdate(e => e.ExpenseId, ex);
            //    //context.ExpenseTypes.AddOrUpdate(et);

            //}

            
            var patients = new List<Patient> {
                new Patient
                {
                    PatientId = 1,
                    PatientName = NewUserList[6].FullName,
                    UserId= NewUserList[6].Id,
                    CustomerId = NewUserList[6].CustomerId
                      },
                new Patient
                {
                    PatientId = 2,
                    PatientName = NewUserList[7].FullName,
                    UserId= NewUserList[7].Id,
                    CustomerId = NewUserList[7].CustomerId
                      },
                 new Patient
                {
                    PatientId = 3,
                    PatientName = NewUserList[8].FullName,
                    UserId= NewUserList[8].Id,
                    CustomerId = NewUserList[8].CustomerId
                 },
                  new Patient
                {
                    PatientId = 4,
                    PatientName = NewUserList[9].FullName,
                    UserId= NewUserList[9].Id,
                    CustomerId = NewUserList[9].CustomerId
                 }
             };

            foreach (var pt in patients)
            {
                context.Patients.AddOrUpdate(p => p.PatientId, pt);
                //context.ExpenseTypes.AddOrUpdate(et);

            }

            var staffRoles = new List<StaffRole> {
                new StaffRole
                {
                    StaffRoleId=1,
                    Name = "Assistent"
                },
                new StaffRole
                {
                    StaffRoleId=2,
                    Name = "Arbetsledare"
                },
                new StaffRole
                {
                    StaffRoleId=3,
                    Name = "Administratör"
                }

            };

            foreach (var sr in staffRoles)
            {
                context.StaffRoles.AddOrUpdate(s => s.StaffRoleId, sr);

            }


            var patientUsers = new List<PatientUser> {
                new PatientUser
                {
                    PatientUserId = 1,
                    PatientId = 1,
                    StaffUserId = NewUserList[3].Id,
                    StaffRoleId =1
                 },
                new PatientUser
                {
                    PatientUserId = 2,
                    PatientId = 2,
                    StaffUserId= NewUserList[4].Id,
                    StaffRoleId =1
                      },
                 new PatientUser
                {
                    PatientUserId = 3,
                    PatientId = 3,
                    StaffUserId= NewUserList[4].Id,
                    StaffRoleId =1
                 },
                 new PatientUser
                {
                    PatientUserId = 4,
                    PatientId = 1,
                    StaffUserId= NewUserList[2].Id,
                    StaffRoleId =2
                      },
                new PatientUser
                {
                    PatientUserId = 5,
                    PatientId = 2,
                    StaffUserId= NewUserList[1].Id,
                    StaffRoleId =2
                      },
                 new PatientUser
                {
                    PatientUserId = 6,
                    PatientId = 3,
                    StaffUserId= NewUserList[1].Id,
                    StaffRoleId =2
                 },
                  new PatientUser
                {
                    PatientUserId = 7,
                    PatientId = 4,
                    StaffUserId= NewUserList[5].Id,
                    StaffRoleId = 1
                      },
                new PatientUser
                {
                    PatientUserId = 8,
                    PatientId = 4,
                    StaffUserId= NewUserList[2].Id,
                    StaffRoleId = 2
                      },
                 new PatientUser
                {
                    PatientUserId = 9,
                    PatientId = 4,
                    StaffUserId= NewUserList[3].Id,
                    StaffRoleId = 1
                  },
                 new PatientUser
                {
                    PatientUserId = 10,
                    PatientId = 1,
                    StaffUserId = NewUserList[0].Id,
                    StaffRoleId =1
                 }
             };

            foreach (var pu in patientUsers)
            {
                context.PatientUsers.AddOrUpdate(p => p.PatientUserId, pu);
                //context.ExpenseTypes.AddOrUpdate(et);

            }

            var note = new List<Note> {
                    new Note
                    {
                        NoteId = 1,
                        NoteTime = DateTime.Now,
                        NoteInfo = "Skickar in" ,
                        NoteStatus = "Ny",
                        TravelReportId= 7,
                        ApplicationUserId = NewUserList[3].FullName
                    },
                    new Note
                    {
                        NoteId = 2,
                        NoteTime = DateTime.Now,
                        NoteInfo = "Inte godkänd",
                        NoteStatus = "Ej godkänd",
                        TravelReportId= 7,
                        ApplicationUserId = NewUserList[2].FullName
                    },
                       new Note
                    {
                        NoteId = 3,
                        NoteTime = DateTime.Now,
                        NoteInfo = "Skickar in igen" ,
                        NoteStatus = "Inskickad",
                        TravelReportId= 7,
                        ApplicationUserId = NewUserList[3].FullName
                    },
                    new Note
                    {
                        NoteId = 4,
                        NoteTime = DateTime.Now,
                        NoteInfo = "Godkänd",
                        NoteStatus = "Godkänd",
                        TravelReportId= 7,
                        ApplicationUserId = NewUserList[10].FullName
                    }
                };

            foreach (var nr in note)
            {
                context.Notes.AddOrUpdate(n => n.NoteId, nr);
            }
        }
    }
}
