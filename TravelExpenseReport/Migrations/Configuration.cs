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

            foreach (string roleName in new[] { "Assistant", "WorkAdministrator" })
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    var role = new IdentityRole { Name = roleName };
                    roleManager.Create(role);
                }

            }


            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var users = new List<ApplicationUser> {
                new ApplicationUser {FullName = "Oscar Antonsson", Email = "oscar.antonsson@ab.se", UserName = "oscar.antonsson@ab.se"},
                new ApplicationUser {FullName = "Allan Persson", Email = "allan.persson@ab.se", UserName = "allan.persson@ab.se"}

            };

            var NewUserList = new List<ApplicationUser>();

            foreach (var u in users)
            {
                userManager.Create(u, "foobar");
                var user = userManager.FindByEmail(u.Email);
                NewUserList.Add(user);
                userManager.AddToRole(user.Id, "WorkAdministrator");
            }


            var users2 = new List<ApplicationUser> {
                new ApplicationUser {FullName = "Lena Källgren", Email = "lena.kallgren@ab.se", UserName = "lena.kallgren@ab.se"},
                new ApplicationUser {FullName = "Rickard Nilsson", Email = "rickard.nilsson@ab.se", UserName = "rickard.nilsson@ab.se"}
            };

            foreach (var u in users2)
            {
                userManager.Create(u, "foobar");
                var user = userManager.FindByEmail(u.Email);
                NewUserList.Add(user);
                userManager.AddToRole(user.Id, "Assistant");
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
                    HalfDayAmount = 100,
                    NightAmount = 100,
                    MilageAmount = 175,
                    BreakfastReductionAmount = 42,
                    LunchReductionAmount = 75,
                    DinnerReductionAmount =75

                },
                new LegalAmount
                {
                   LegalAmountId = 2,
                    ValidDate = DateTime.Parse("2015-01-01"),
                    FullDayAmount = 220,
                    HalfDayAmount = 110,
                    NightAmount = 110,
                    MilageAmount = 185,
                    BreakfastReductionAmount = 44,
                    LunchReductionAmount = 77,
                    DinnerReductionAmount =77
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
                    TravelReportName = "2016-001",
                    Destination = "Flen",
                    Purpose = "Utbildning",
                    DepartureDate = DateTime.Parse("2016-04-20 00:00:00"),
                    DepartureTime = TimeSpan.Parse("13:00:00"),
                    ReturnDate = DateTime.Parse("2016-04-22 00:00:00"),
                    ReturnTime = TimeSpan.Parse("16:00:00"),
                    DepartureHoursExtra = 1 ,
                    ReturnHoursExtra = 2,
                    FullDay = 0,
                    HalfDay = 0,
                    Night = 0,
                    BreakfastReduction = 0,
                    LunchReduction = 0,
                    DinnerReduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 2,
                    //ApplicationUserId = "7bce74df-983d-4b45-81d5-530634133665",
                    ApplicationUserId = NewUserList[1].Id,
                    TravelReportName = "2016-002",
                    Destination = "Malmö",
                    Purpose = "Studiebesök",
                    DepartureDate = DateTime.Parse("2016-04-23 00:00:00"),
                    DepartureTime = TimeSpan.Parse("08:30:00"),
                    ReturnDate = DateTime.Parse("2016-04-27 00:00:00"),
                    ReturnTime = TimeSpan.Parse("20:00:00"),
                    DepartureHoursExtra = 1 ,
                    ReturnHoursExtra = 2,
                    FullDay = 0,
                    HalfDay = 0,
                    Night = 0,
                    BreakfastReduction = 0,
                    LunchReduction = 1,
                    DinnerReduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 3,
                    //ApplicationUserId = "855555ef-8f46-4281-9161-5777699b4d2d",
                    ApplicationUserId = NewUserList[3].Id,
                    TravelReportName = "2016-001",
                    Destination = "Uppsala",
                    Purpose = "Läger",
                    DepartureDate = DateTime.Parse("2016-04-20 00:00:00"),
                    DepartureTime = TimeSpan.Parse("17:45:00"),
                    ReturnDate = DateTime.Parse("2016-04-22 00:00:00"),
                    ReturnTime = TimeSpan.Parse("07:30:00"),
                    DepartureHoursExtra = 2 ,
                    ReturnHoursExtra = 0,
                    FullDay = 0,
                    HalfDay = 0,
                    Night = 0,
                    BreakfastReduction = 0,
                    LunchReduction = 0,
                    DinnerReduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 4,
                    //ApplicationUserId = "855555ef-8f46-4281-9161-5777699b4d2d",
                    ApplicationUserId = NewUserList[2].Id,
                    TravelReportName = "2016-002",
                    Destination = "Sundsvall",
                    Purpose = "Besöka släkt över Valborrg",
                    DepartureDate = DateTime.Parse("2016-04-29 00:00:00"),
                    DepartureTime = TimeSpan.Parse("11:59:00"),
                    ReturnDate = DateTime.Parse("2016-05-01 00:00:00"),
                    ReturnTime = TimeSpan.Parse("18:30:00"),
                    DepartureHoursExtra = 2 ,
                    ReturnHoursExtra = 0,
                    FullDay = 0,
                    HalfDay = 0,
                    Night = 0,
                    BreakfastReduction = 0,
                    LunchReduction = 0,
                    DinnerReduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                 new TravelReport
                {
                    TravelReportId = 5,
                    //ApplicationUserId = "cb791d4e-92a8-41ba-aeb3-be2d3000af15",
                    ApplicationUserId = NewUserList[1].Id,
                    TravelReportName = "2016-001",
                    Destination = "Göteborg",
                    Purpose = "Besök på Liseberg",
                    DepartureDate = DateTime.Parse("2016-04-24 00:00:00"),
                    DepartureTime = TimeSpan.Parse("06:00:00"),
                    ReturnDate = DateTime.Parse("2016-04-27 00:00:00"),
                    ReturnTime = TimeSpan.Parse("17:00:00"),
                    DepartureHoursExtra = 0 ,
                    ReturnHoursExtra = 2,
                    FullDay = 0,
                    HalfDay = 0,
                    Night = 0,
                    BreakfastReduction = 0,
                    LunchReduction = 0,
                    DinnerReduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 6,
                    //ApplicationUserId = "cb791d4e-92a8-41ba-aeb3-be2d3000af15",
                    ApplicationUserId = NewUserList[1].Id,
                    TravelReportName = "2016-002",
                    Destination = "Västerås",
                    Purpose = "Bandymatch",
                    DepartureDate = DateTime.Parse("2016-04-28 00:00:00"),
                    DepartureTime = TimeSpan.Parse("08:30:00"),
                    ReturnDate = DateTime.Parse("2016-04-29 00:00:00"),
                    ReturnTime = TimeSpan.Parse("18:30:00"),
                    DepartureHoursExtra = 1 ,
                    ReturnHoursExtra = 2,
                    FullDay = 0,
                    HalfDay = 0,
                    Night = 0,
                    BreakfastReduction = 0,
                    LunchReduction = 1,
                    DinnerReduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 7,
                    //ApplicationUserId = "f77513f6-4c8b-4eb2-9896-b292dd9a294e",
                    ApplicationUserId = NewUserList[3].Id,
                    TravelReportName = "2016-001",
                    Destination = "Enköping",
                    Purpose = "Studiebesök boende",
                    DepartureDate = DateTime.Parse("2016-04-26 00:00:00"),
                    DepartureTime = TimeSpan.Parse("17:45:00"),
                    ReturnDate = DateTime.Parse("2016-04-27 00:00:00"),
                    ReturnTime = TimeSpan.Parse("07:30:00"),
                    DepartureHoursExtra = 2 ,
                    ReturnHoursExtra = 0,
                    FullDay = 0,
                    HalfDay = 0,
                    Night = 0,
                    BreakfastReduction = 0,
                    LunchReduction = 0,
                    DinnerReduction = 0,
                    StatusTypeId = 1,
                    Comment = null
                 },
                new TravelReport
                {
                    TravelReportId = 8,
                    //ApplicationUserId = "f77513f6-4c8b-4eb2-9896-b292dd9a294e",
                    ApplicationUserId = NewUserList[3].Id,
                    TravelReportName = "2016-002",
                    Destination = "Sundsvall",
                    Purpose = "Besöka släkt",
                    DepartureDate = DateTime.Parse("2016-05-02 00:00:00"),
                    DepartureTime = TimeSpan.Parse("10:30:00"),
                    ReturnDate = DateTime.Parse("2016-05-07 00:00:00"),
                    ReturnTime = TimeSpan.Parse("19:30:00"),
                    DepartureHoursExtra = 0,
                    ReturnHoursExtra = 2,
                    FullDay = 0,
                    HalfDay = 0,
                    Night = 0,
                    BreakfastReduction = 0,
                    LunchReduction = 0,
                    DinnerReduction = 0,
                    StatusTypeId = 2,
                    Comment = null
                 }
                };



            foreach (var tr in travelReport)
            {
                context.TravelReports.AddOrUpdate(t => t.TravelReportId, tr);
                //context.ExpenseTypes.AddOrUpdate(et);

            }


            var expenses = new List<Expense> {
                    new Expense
                    {
                        ExpenseId = 1,
                        ExpenseTypeId = 1,
                        ExpenseInformation = null,
                        ExpenseDate = DateTime.Parse("2016-04-20"),
                        ExpenseAmount = 345,
                        ExpenseMilage = 0,
                        TravelReportId = 1

                    },
                     new Expense
                    {
                        ExpenseId = 2,
                        ExpenseTypeId = 3,
                        ExpenseInformation = null,
                        ExpenseDate = DateTime.Parse("2014-04-20"),
                        ExpenseAmount = 345,
                        ExpenseMilage = 0,
                        TravelReportId = 1

                    },
                      new Expense
                    {
                        ExpenseId = 3,
                        ExpenseTypeId = 2,
                        ExpenseInformation = null,
                        ExpenseDate = DateTime.Parse("2014-04-23"),
                        ExpenseAmount = 2550,
                        ExpenseMilage = 0,
                        TravelReportId =2

                    },
                    new Expense
                    {
                        ExpenseId = 4,
                        ExpenseTypeId= 2,
                        ExpenseInformation = null,
                        ExpenseDate = DateTime.Parse("2014-04-29"),
                        ExpenseAmount = 3100,
                        ExpenseMilage = 0,
                        TravelReportId = 5,

                    },
                    new Expense
                    {
                        ExpenseId = 5,
                        ExpenseTypeId = 4,
                        ExpenseInformation = null,
                        ExpenseDate = DateTime.Parse("2014-04-25"),
                        ExpenseAmount = 0,
                        ExpenseMilage = 485,
                        TravelReportId = 5,
                    },
                         new Expense
                    {
                        ExpenseId = 6,
                        ExpenseTypeId = 4,
                        ExpenseInformation = null,
                        ExpenseDate = DateTime.Parse("2014-04-27"),
                        ExpenseAmount = 0,
                        ExpenseMilage = 375,
                        TravelReportId = 6,
                     },
                         new Expense
                    {
                        ExpenseId = 7,
                        ExpenseTypeId = 2,
                        ExpenseInformation = null,
                        ExpenseDate = DateTime.Parse("2014-05-02"),
                        ExpenseAmount = 5630,
                        ExpenseMilage = 0,
                        TravelReportId = 8,

                    },
                          new Expense
                    {
                        ExpenseId = 8,
                        ExpenseTypeId = 3,
                        ExpenseInformation = null,
                        ExpenseDate = DateTime.Parse("2014-05-02"),
                        ExpenseAmount = 367,
                        ExpenseMilage = 0,
                        TravelReportId = 8,
                     },
                          new Expense
                    {
                        ExpenseId = 7,
                        ExpenseTypeId = 1,
                        ExpenseInformation = null,
                        ExpenseDate = DateTime.Parse("2014-05-07"),
                        ExpenseAmount = 569,
                        ExpenseMilage = 0,
                        TravelReportId = 8,
                    },
                          new Expense
                    {
                        ExpenseId = 8,
                        ExpenseTypeId = 3,
                        ExpenseInformation = null,
                        ExpenseDate = DateTime.Parse("2014-05-07"),
                        ExpenseAmount = 235,
                        ExpenseMilage = 0,
                        TravelReportId = 8,
                    }
                };

            foreach (var ex in expenses)
            {
                context.Expenses.AddOrUpdate(e => e.ExpenseId, ex);
                //context.ExpenseTypes.AddOrUpdate(et);

            }


            context.SaveChanges();




        }

    }
}
