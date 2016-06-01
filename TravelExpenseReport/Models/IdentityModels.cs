using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel;

namespace TravelExpenseReport.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Namn")]
        public string FullName { get; set; }
        public int CustomerId { get; set; }
        public string EmployeeNo { get; set; }

        public virtual ICollection<TravelReport> TravelReports { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<TravelExpenseReport.Models.Expense> Expenses { get; set; }

        public System.Data.Entity.DbSet<TravelExpenseReport.Models.TravelReport> TravelReports { get; set; }

        public System.Data.Entity.DbSet<TravelExpenseReport.Models.LegalAmount> LegalAmounts { get; set; }

        public System.Data.Entity.DbSet<TravelExpenseReport.Models.Patient> Patients { get; set; }

        public System.Data.Entity.DbSet<TravelExpenseReport.Models.ExpenseType> ExpenseTypes { get; set; }

        public System.Data.Entity.DbSet<TravelExpenseReport.Models.StatusType> StatusTypes { get; set; }

        public System.Data.Entity.DbSet<TravelExpenseReport.Models.StaffRole> StaffRoles { get; set; }

        public System.Data.Entity.DbSet<TravelExpenseReport.Models.PatientUser> PatientUsers { get; set; }

    

    }
}