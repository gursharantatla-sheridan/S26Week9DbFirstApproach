using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace S26Week9DbFirstApproach;

public partial class SchoolContext : DbContext
{
    public SchoolContext()
    {
    }

    public SchoolContext(DbContextOptions<SchoolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Standard> Standards { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentAddress> StudentAddresses { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connStr = ConfigurationManager.ConnectionStrings["School"].ConnectionString;
        optionsBuilder.UseSqlServer(connStr);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D71A77C8FB65D");

            entity.ToTable("Course");

            entity.Property(e => e.CourseName).HasMaxLength(50);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Course_Teacher");
        });

        modelBuilder.Entity<Standard>(entity =>
        {
            entity.HasKey(e => e.StandardId).HasName("PK__Standard__BB33D20C92E8E7F1");

            entity.ToTable("Standard");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.StandardName).HasMaxLength(50);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52B992D0B4CFE");

            entity.ToTable("Student");

            entity.Property(e => e.StudentName).HasMaxLength(50);

            entity.HasOne(d => d.Standard).WithMany(p => p.Students)
                .HasForeignKey(d => d.StandardId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Student_Standard");

            entity.HasMany(d => d.Courses).WithMany(p => p.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_StudentCourse_Course"),
                    l => l.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK_StudentCourse_Student"),
                    j =>
                    {
                        j.HasKey("StudentId", "CourseId").HasName("PK__StudentC__5E57FC8323ADD48D");
                        j.ToTable("StudentCourse");
                    });
        });

        modelBuilder.Entity<StudentAddress>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__StudentA__32C52B9979961B75");

            entity.ToTable("StudentAddress");

            entity.Property(e => e.StudentId).ValueGeneratedNever();
            entity.Property(e => e.Address1).HasMaxLength(50);
            entity.Property(e => e.Address2).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Province).HasMaxLength(50);

            entity.HasOne(d => d.Student).WithOne(p => p.StudentAddress)
                .HasForeignKey<StudentAddress>(d => d.StudentId)
                .HasConstraintName("FK_StudentAddress_Student");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teacher__EDF25964E48E9C93");

            entity.ToTable("Teacher");

            entity.Property(e => e.TeacherName).HasMaxLength(50);

            entity.HasOne(d => d.Standard).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.StandardId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Teacher_Standard");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
