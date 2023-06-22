﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Infrastructure.DataModels;

namespace Infrastructure.Data
{
    public partial class YGCContext : DbContext
    {
        public YGCContext()
        {
        }

        public YGCContext(DbContextOptions<YGCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AvailableDate> AvailableDates { get; set; } = null!;
        public virtual DbSet<ChangeClassRequest> ChangeClassRequests { get; set; } = null!;
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local); database = YGC ;uid=sa;pwd=123456789;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AvailableDate>(entity =>
            {
                entity.HasKey(e => new { e.LecturerId, e.SlotId })
                    .HasName("PK__Availabl__7DA07AAAAFCDBABB");

                entity.ToTable("AvailableDate");

                entity.Property(e => e.LecturerId).HasColumnName("lecturer_id");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.AvailableDates)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Available__lectu__30F848ED");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.AvailableDates)
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Available__slot___3A81B327");
            });

            modelBuilder.Entity<ChangeClassRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("PK__ChangeCl__18D3B90FAB62976F");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.ClassId).HasColumnName("class_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(500)
                    .HasColumnName("content");

                entity.Property(e => e.IsApproved).HasColumnName("is_approved");

                entity.Property(e => e.RequestClassId).HasColumnName("request_class_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ChangeClassRequestClasses)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__ChangeCla__class__3B75D760");

                entity.HasOne(d => d.RequestClass)
                    .WithMany(p => p.ChangeClassRequestRequestClasses)
                    .HasForeignKey(d => d.RequestClassId)
                    .HasConstraintName("FK__ChangeCla__reque__52593CB8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ChangeClassRequests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__ChangeCla__user___3C69FB99");
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
                    .HasName("PK__DateOfWe__8B516ABB7F2E9CA2");

                entity.ToTable("DateOfWeek");

                entity.Property(e => e.DayId)
                    .ValueGeneratedNever()
                    .HasColumnName("day_id");

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
                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("role_id");

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
                    .HasConstraintName("FK__Schedule__class___4316F928");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.SlotId)
                    .HasConstraintName("FK__Schedule__slot_i__440B1D61");
            });

            modelBuilder.Entity<StudySlot>(entity =>
            {
                entity.HasKey(e => e.SlotId)
                    .HasName("PK__StudySlo__971A01BB229BD5A6");

                entity.ToTable("StudySlot");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.HasMany(d => d.Days)
                    .WithMany(p => p.Slots)
                    .UsingEntity<Dictionary<string, object>>(
                        "StudySlotDay",
                        l => l.HasOne<DateOfWeek>().WithMany().HasForeignKey("DayId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__StudySlot__day_i__2E1BDC42"),
                        r => r.HasOne<StudySlot>().WithMany().HasForeignKey("SlotId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__StudySlot__slot___45F365D3"),
                        j =>
                        {
                            j.HasKey("SlotId", "DayId").HasName("PK__StudySlo__3FAF17102EA82550");

                            j.ToTable("StudySlotDay");

                            j.IndexerProperty<int>("SlotId").HasColumnName("slot_id");

                            j.IndexerProperty<int>("DayId").HasColumnName("day_id");
                        });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .HasName("PK__Users__DD701264B374D8F6");

                entity.Property(e => e.Uid).HasColumnName("uid");

                entity.Property(e => e.Address)
                    .HasMaxLength(150)
                    .HasColumnName("address");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .HasColumnName("full_name");

                entity.Property(e => e.IsVerified).HasColumnName("is_verified");

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
                        l => l.HasOne<Class>().WithMany().HasForeignKey("ClassId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Enrollmen__class__3E52440B"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Enrollmen__stude__36B12243"),
                        j =>
                        {
                            j.HasKey("StudentId", "ClassId").HasName("PK__Enrollme__55EC4102536F4D2E");

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
