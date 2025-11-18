using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectDataManager.Model;

namespace ProjectDataManager.DataAccess;

public class ProjectDataManagerDbContext : IdentityDbContext<Employee, IdentityRole<int>, int>
{
    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectEmployee> ProjectEmployees { get; set; }

    public virtual DbSet<ProjectTask> Tasks { get; set; }

    public ProjectDataManagerDbContext(DbContextOptions<ProjectDataManagerDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("EmployeeRoles");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("Claims");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("Logins");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("Tokens");

        modelBuilder.Entity<Employee>(b =>
        {
            b.Property(b => b.FirstName)
                .HasMaxLength(50);

            b.Property(b => b.LastName)
                .HasMaxLength(50);

            b.Property(b => b.MiddleName)
                .HasMaxLength(50);

            b.Property(b => b.Email)
                .IsRequired()
                .HasMaxLength(50);

            b.HasOne(e => e.Manager)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            b.ToTable("Employee");
        });

        modelBuilder.Entity<Project>(b =>
        {
            b.Property(b => b.Name)
                .HasMaxLength(100);

            b.Property(b => b.CustomerCompany)
                .HasMaxLength(100);

            b.Property(b => b.ContractorCompany)
                .HasMaxLength(100);

            b.HasOne(p => p.ProjectManager)
                .WithMany(e => e.ManagedProjects)
                .HasForeignKey(p => p.ProjectManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProjectEmployee>(b =>
        {
            b.HasOne(pe => pe.Employee)
                .WithMany(e => e.ProjectEmployees)
                .HasForeignKey(pe => pe.EmployeeId);

            b.HasOne(pe => pe.Project)
                .WithMany(p => p.ProjectEmployees)
                .HasForeignKey(pe => pe.ProjectId);
        });

        modelBuilder.Entity<ProjectTask>(b =>
        {
            b.Property(b => b.Name)
                .HasMaxLength(50);

            b.Property(b => b.Comment)
                .HasMaxLength(1000);

            b.HasOne(t => t.Author)
                .WithMany(a => a.AuthoredTasks)
                .HasForeignKey(t => t.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(t => t.Executor)
                .WithMany(e => e.ExecutingTasks)
                .HasForeignKey(t => t.ExecutorId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId);
        });
    }
}
