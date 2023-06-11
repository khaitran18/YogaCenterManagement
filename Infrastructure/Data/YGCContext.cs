using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Infrastructure.DataModels;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public partial class YGCContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public YGCContext()
        {
        }

        public YGCContext(DbContextOptions<YGCContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<AvailableDate> AvailableDates { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<DateOfWeek> DateOfWeeks { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<StudySlot> StudySlots { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _configuration.GetConnectionString("YGC");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AvailableDate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AvailableDate");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.LecturerId).HasColumnName("lecturer_id");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.HasOne(d => d.Lecturer)
                    .WithMany()
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK__Available__lectu__30F848ED");

                entity.HasOne(d => d.Slot)
                    .WithMany()
                    .HasForeignKey(d => d.SlotId)
                    .HasConstraintName("FK__Available__slot___300424B4");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.Property(e => e.ClassId).HasColumnName("class_id");

                entity.Property(e => e.ClassName)
                    .HasMaxLength(255)
                    .HasColumnName("class_name");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.LecturerId).HasColumnName("lecturer_id");

                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK__Class__lecturer___33D4B598");
            });

            modelBuilder.Entity<DateOfWeek>(entity =>
            {
                entity.HasKey(e => e.DayId)
                    .HasName("PK__DateOfWe__8B516ABBDB037D7D");

                entity.ToTable("DateOfWeek");

                entity.Property(e => e.DayId).HasColumnName("day_id");

                entity.Property(e => e.Day)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("day");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasColumnName("content");

                entity.Property(e => e.LecturerId).HasColumnName("lecturer_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.FeedbackLecturers)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK__Feedback__lectur__3F466844");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.FeedbackStudents)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__Feedback__studen__3E52440B");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.Method)
                    .HasMaxLength(50)
                    .HasColumnName("method");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__Payment__student__4222D4EF");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassId).HasColumnName("class_id");

                entity.Property(e => e.DailyNote)
                    .HasMaxLength(255)
                    .HasColumnName("daily_note");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__Schedule__class___3A81B327");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.SlotId)
                    .HasConstraintName("FK__Schedule__slot_i__3B75D760");
            });

            modelBuilder.Entity<StudySlot>(entity =>
            {
                entity.HasKey(e => e.SlotId)
                    .HasName("PK__StudySlo__971A01BB95A393F6");

                entity.ToTable("StudySlot");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.HasMany(d => d.Days)
                    .WithMany(p => p.Slots)
                    .UsingEntity<Dictionary<string, object>>(
                        "StudySlotDay",
                        l => l.HasOne<DateOfWeek>().WithMany().HasForeignKey("DayId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__StudySlot__day_i__2E1BDC42"),
                        r => r.HasOne<StudySlot>().WithMany().HasForeignKey("SlotId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__StudySlot__slot___2D27B809"),
                        j =>
                        {
                            j.HasKey("SlotId", "DayId").HasName("PK__StudySlo__3FAF171003942FE4");

                            j.ToTable("StudySlotDay");

                            j.IndexerProperty<int>("SlotId").HasColumnName("slot_id");

                            j.IndexerProperty<int>("DayId").HasColumnName("day_id");
                        });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .HasName("PK__Users__DD701264886CF1AD");

                entity.Property(e => e.Uid).HasColumnName("uid");

                entity.Property(e => e.Address)
                    .HasMaxLength(150)
                    .HasColumnName("address");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .HasColumnName("full_name");

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__role_id__267ABA7A");

                entity.HasMany(d => d.ClassesNavigation)
                    .WithMany(p => p.Students)
                    .UsingEntity<Dictionary<string, object>>(
                        "Enrollment",
                        l => l.HasOne<Class>().WithMany().HasForeignKey("ClassId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Enrollmen__class__37A5467C"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Enrollmen__stude__36B12243"),
                        j =>
                        {
                            j.HasKey("StudentId", "ClassId").HasName("PK__Enrollme__55EC410211EB7C3A");

                            j.ToTable("Enrollment");

                            j.IndexerProperty<int>("StudentId").HasColumnName("student_id");

                            j.IndexerProperty<int>("ClassId").HasColumnName("class_id");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
