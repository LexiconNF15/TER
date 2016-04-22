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

            context.SaveChanges();
        }

    }
}
