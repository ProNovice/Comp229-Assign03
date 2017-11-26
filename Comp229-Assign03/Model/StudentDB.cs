namespace Comp229_Assign03.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StudentDB : DbContext
    {
        public StudentDB()
            : base("name=StudentDB")
        {
        }

        public virtual DbSet<Cours> Courses { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cours>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Cours>()
                .HasMany(e => e.Enrollments)
                .WithRequired(e => e.Cours)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Courses)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.FirstMidName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Enrollments)
                .WithRequired(e => e.Student)
                .WillCascadeOnDelete(false);
        }
    }
}
