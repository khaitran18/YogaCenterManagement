﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(YGCContext))]
    partial class YGCContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Enrollment", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("student_id");

                    b.Property<int>("ClassId")
                        .HasColumnType("int")
                        .HasColumnName("class_id");

                    b.HasKey("StudentId", "ClassId")
                        .HasName("PK__Enrollme__55EC41027C0B47FF");

                    b.HasIndex("ClassId");

                    b.ToTable("Enrollment", (string)null);
                });

            modelBuilder.Entity("Infrastructure.DataModels.AvailableDate", b =>
                {
                    b.Property<int>("LecturerId")
                        .HasColumnType("int")
                        .HasColumnName("lecturer_id");

                    b.Property<int>("SlotId")
                        .HasColumnType("int")
                        .HasColumnName("slot_id");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime")
                        .HasColumnName("date");

                    b.HasKey("LecturerId", "SlotId")
                        .HasName("PK__Availabl__7DA07AAACB79894C");

                    b.HasIndex("SlotId");

                    b.ToTable("AvailableDate", (string)null);
                });

            modelBuilder.Entity("Infrastructure.DataModels.ChangeClassRequest", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("request_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequestId"), 1L, 1);

                    b.Property<int?>("ClassId")
                        .HasColumnType("int")
                        .HasColumnName("class_id");

                    b.Property<string>("Content")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("content");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit")
                        .HasColumnName("is_approved");

                    b.Property<int?>("RequestClassId")
                        .HasColumnType("int")
                        .HasColumnName("request_class_id");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("RequestId")
                        .HasName("PK__ChangeCl__18D3B90FDC679E9F");

                    b.HasIndex("ClassId");

                    b.HasIndex("RequestClassId");

                    b.HasIndex("UserId");

                    b.ToTable("ChangeClassRequests");
                });

            modelBuilder.Entity("Infrastructure.DataModels.Class", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("class_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassId"), 1L, 1);

                    b.Property<int>("ClassCapacity")
                        .HasColumnType("int")
                        .HasColumnName("class_capacity");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("class_name");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("end_date");

                    b.Property<int?>("LecturerId")
                        .HasColumnType("int")
                        .HasColumnName("lecturer_id");

                    b.Property<double>("Price")
                        .HasColumnType("float")
                        .HasColumnName("price");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("start_date");

                    b.HasKey("ClassId");

                    b.HasIndex("LecturerId");

                    b.ToTable("Class", (string)null);
                });

            modelBuilder.Entity("Infrastructure.DataModels.DateOfWeek", b =>
                {
                    b.Property<int>("DayId")
                        .HasColumnType("int")
                        .HasColumnName("day_id");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("day");

                    b.HasKey("DayId")
                        .HasName("PK__DateOfWe__8B516ABB7F2E9CA2");

                    b.ToTable("DateOfWeek", (string)null);
                });

            modelBuilder.Entity("Infrastructure.DataModels.Feedback", b =>
                {
                    b.Property<int>("FeedbackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("feedback_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FeedbackId"), 1L, 1);

                    b.Property<string>("Content")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("content");

                    b.Property<int?>("LecturerId")
                        .HasColumnType("int")
                        .HasColumnName("lecturer_id");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("student_id");

                    b.Property<DateTime?>("Time")
                        .HasColumnType("datetime2")
                        .HasColumnName("time");

                    b.HasKey("FeedbackId");

                    b.HasIndex("LecturerId");

                    b.HasIndex("StudentId");

                    b.ToTable("Feedback", (string)null);
                });

            modelBuilder.Entity("Infrastructure.DataModels.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("payment_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("money")
                        .HasColumnName("amount");

                    b.Property<string>("Method")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("method");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("student_id");

                    b.HasKey("PaymentId");

                    b.HasIndex("StudentId");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("Infrastructure.DataModels.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("role_name");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Infrastructure.DataModels.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ClassId")
                        .HasColumnType("int")
                        .HasColumnName("class_id");

                    b.Property<string>("DailyNote")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("daily_note");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("date");

                    b.Property<int?>("SlotId")
                        .HasColumnType("int")
                        .HasColumnName("slot_id");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("SlotId");

                    b.ToTable("Schedule", (string)null);
                });

            modelBuilder.Entity("Infrastructure.DataModels.StudySlot", b =>
                {
                    b.Property<int>("SlotId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("slot_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SlotId"), 1L, 1);

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time")
                        .HasColumnName("end_time");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time")
                        .HasColumnName("start_time");

                    b.HasKey("SlotId")
                        .HasName("PK__StudySlo__971A01BB6CD92A1C");

                    b.ToTable("StudySlot", (string)null);
                });

            modelBuilder.Entity("Infrastructure.DataModels.User", b =>
                {
                    b.Property<int>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("uid");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Uid"), 1L, 1);

                    b.Property<string>("Address")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("address");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("full_name");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit")
                        .HasColumnName("is_verified");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .HasMaxLength(12)
                        .IsUnicode(false)
                        .HasColumnType("varchar(12)")
                        .HasColumnName("phone");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("user_name");

                    b.HasKey("Uid")
                        .HasName("PK__Users__DD701264B374D8F6");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StudySlotDay", b =>
                {
                    b.Property<int>("SlotId")
                        .HasColumnType("int")
                        .HasColumnName("slot_id");

                    b.Property<int>("DayId")
                        .HasColumnType("int")
                        .HasColumnName("day_id");

                    b.HasKey("SlotId", "DayId")
                        .HasName("PK__StudySlo__3FAF1710B9F5AE59");

                    b.HasIndex("DayId");

                    b.ToTable("StudySlotDay", (string)null);
                });

            modelBuilder.Entity("Enrollment", b =>
                {
                    b.HasOne("Infrastructure.DataModels.Class", null)
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .IsRequired()
                        .HasConstraintName("FK__Enrollmen__class__787EE5A0");

                    b.HasOne("Infrastructure.DataModels.User", null)
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK__Enrollmen__stude__36B12243");
                });

            modelBuilder.Entity("Infrastructure.DataModels.AvailableDate", b =>
                {
                    b.HasOne("Infrastructure.DataModels.User", "Lecturer")
                        .WithMany("AvailableDates")
                        .HasForeignKey("LecturerId")
                        .IsRequired()
                        .HasConstraintName("FK__Available__lectu__30F848ED");

                    b.HasOne("Infrastructure.DataModels.StudySlot", "Slot")
                        .WithMany("AvailableDates")
                        .HasForeignKey("SlotId")
                        .IsRequired()
                        .HasConstraintName("FK__Available__slot___73BA3083");

                    b.Navigation("Lecturer");

                    b.Navigation("Slot");
                });

            modelBuilder.Entity("Infrastructure.DataModels.ChangeClassRequest", b =>
                {
                    b.HasOne("Infrastructure.DataModels.Class", "Class")
                        .WithMany("ChangeClassRequestClasses")
                        .HasForeignKey("ClassId")
                        .HasConstraintName("FK__ChangeCla__class__74AE54BC");

                    b.HasOne("Infrastructure.DataModels.Class", "RequestClass")
                        .WithMany("ChangeClassRequestRequestClasses")
                        .HasForeignKey("RequestClassId")
                        .HasConstraintName("FK__ChangeCla__reque__75A278F5");

                    b.HasOne("Infrastructure.DataModels.User", "User")
                        .WithMany("ChangeClassRequests")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__ChangeCla__user___76969D2E");

                    b.Navigation("Class");

                    b.Navigation("RequestClass");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Infrastructure.DataModels.Class", b =>
                {
                    b.HasOne("Infrastructure.DataModels.User", "Lecturer")
                        .WithMany("Classes")
                        .HasForeignKey("LecturerId")
                        .HasConstraintName("FK__Class__lecturer___33D4B598");

                    b.Navigation("Lecturer");
                });

            modelBuilder.Entity("Infrastructure.DataModels.Feedback", b =>
                {
                    b.HasOne("Infrastructure.DataModels.User", "Lecturer")
                        .WithMany("FeedbackLecturers")
                        .HasForeignKey("LecturerId")
                        .HasConstraintName("FK__Feedback__lectur__3F466844");

                    b.HasOne("Infrastructure.DataModels.User", "Student")
                        .WithMany("FeedbackStudents")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK__Feedback__studen__3E52440B");

                    b.Navigation("Lecturer");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Infrastructure.DataModels.Payment", b =>
                {
                    b.HasOne("Infrastructure.DataModels.User", "Student")
                        .WithMany("Payments")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK__Payment__student__4222D4EF");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Infrastructure.DataModels.Schedule", b =>
                {
                    b.HasOne("Infrastructure.DataModels.Class", "Class")
                        .WithMany("Schedules")
                        .HasForeignKey("ClassId")
                        .HasConstraintName("FK__Schedule__class___7D439ABD");

                    b.HasOne("Infrastructure.DataModels.StudySlot", "Slot")
                        .WithMany("Schedules")
                        .HasForeignKey("SlotId")
                        .HasConstraintName("FK__Schedule__slot_i__7E37BEF6");

                    b.Navigation("Class");

                    b.Navigation("Slot");
                });

            modelBuilder.Entity("Infrastructure.DataModels.User", b =>
                {
                    b.HasOne("Infrastructure.DataModels.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__Users__role_id__267ABA7A");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("StudySlotDay", b =>
                {
                    b.HasOne("Infrastructure.DataModels.DateOfWeek", null)
                        .WithMany()
                        .HasForeignKey("DayId")
                        .IsRequired()
                        .HasConstraintName("FK__StudySlot__day_i__2E1BDC42");

                    b.HasOne("Infrastructure.DataModels.StudySlot", null)
                        .WithMany()
                        .HasForeignKey("SlotId")
                        .IsRequired()
                        .HasConstraintName("FK__StudySlot__slot___00200768");
                });

            modelBuilder.Entity("Infrastructure.DataModels.Class", b =>
                {
                    b.Navigation("ChangeClassRequestClasses");

                    b.Navigation("ChangeClassRequestRequestClasses");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("Infrastructure.DataModels.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Infrastructure.DataModels.StudySlot", b =>
                {
                    b.Navigation("AvailableDates");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("Infrastructure.DataModels.User", b =>
                {
                    b.Navigation("AvailableDates");

                    b.Navigation("ChangeClassRequests");

                    b.Navigation("Classes");

                    b.Navigation("FeedbackLecturers");

                    b.Navigation("FeedbackStudents");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
