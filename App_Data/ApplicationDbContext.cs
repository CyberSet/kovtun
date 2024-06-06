using System.Data.Entity;
using kovtun.Models;

namespace kovtun.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Workplace>()
                .HasMany(w => w.Operations)
                .WithRequired(o => o.Workplace)
                .HasForeignKey(o => o.WorkplaceId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Operations)
                .WithRequired(o => o.Employee)
                .HasForeignKey(o => o.EmployeeId);
        }
    }
}
