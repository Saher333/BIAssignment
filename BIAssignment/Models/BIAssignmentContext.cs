using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BIAssignment.Models
{
    public partial class BIAssignmentContext : DbContext
    {
        public BIAssignmentContext()
        {
        }

        public BIAssignmentContext(DbContextOptions<BIAssignmentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Benefit> Benefit { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeBenefit> EmployeeBenefit { get; set; }
        public virtual DbSet<SystemConfig> SystemConfig { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-B8IIS2NH\\SQLEXPRESS;Initial Catalog=B&IAssignment;Trusted_Connection=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Benefit>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmployeeBenefit>(entity =>
            {
                entity.HasOne(d => d.Benefit)
                    .WithMany(p => p.EmployeeBenefit)
                    .HasForeignKey(d => d.BenefitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeBenefit_Benefit");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeBenefit)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeBenefit_Employee");
            });

            modelBuilder.Entity<SystemConfig>(entity =>
            {
                entity.Property(e => e.ConfigData).IsUnicode(false);
            });
        }
    }
}
