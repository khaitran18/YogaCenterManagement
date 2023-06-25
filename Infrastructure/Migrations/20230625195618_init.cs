using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DateOfWeek",
                columns: table => new
                {
                    day_id = table.Column<int>(type: "int", nullable: false),
                    day = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DateOfWe__8B516ABB7F2E9CA2", x => x.day_id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false),
                    role_name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "StudySlot",
                columns: table => new
                {
                    slot_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    start_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    end_time = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StudySlo__971A01BB229BD5A6", x => x.slot_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    phone = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: true),
                    is_verified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__DD701264B374D8F6", x => x.uid);
                    table.ForeignKey(
                        name: "FK__Users__role_id__267ABA7A",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "StudySlotDay",
                columns: table => new
                {
                    slot_id = table.Column<int>(type: "int", nullable: false),
                    day_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StudySlo__3FAF17102EA82550", x => new { x.slot_id, x.day_id });
                    table.ForeignKey(
                        name: "FK__StudySlot__day_i__2E1BDC42",
                        column: x => x.day_id,
                        principalTable: "DateOfWeek",
                        principalColumn: "day_id");
                    table.ForeignKey(
                        name: "FK__StudySlot__slot___45F365D3",
                        column: x => x.slot_id,
                        principalTable: "StudySlot",
                        principalColumn: "slot_id");
                });

            migrationBuilder.CreateTable(
                name: "AvailableDate",
                columns: table => new
                {
                    lecturer_id = table.Column<int>(type: "int", nullable: false),
                    slot_id = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Availabl__7DA07AAAAFCDBABB", x => new { x.lecturer_id, x.slot_id });
                    table.ForeignKey(
                        name: "FK__Available__lectu__30F848ED",
                        column: x => x.lecturer_id,
                        principalTable: "Users",
                        principalColumn: "uid");
                    table.ForeignKey(
                        name: "FK__Available__slot___3A81B327",
                        column: x => x.slot_id,
                        principalTable: "StudySlot",
                        principalColumn: "slot_id");
                });

            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    class_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    class_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lecturer_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.class_id);
                    table.ForeignKey(
                        name: "FK__Class__lecturer___33D4B598",
                        column: x => x.lecturer_id,
                        principalTable: "Users",
                        principalColumn: "uid");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    feedback_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    content = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    student_id = table.Column<int>(type: "int", nullable: true),
                    lecturer_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.feedback_id);
                    table.ForeignKey(
                        name: "FK__Feedback__lectur__3F466844",
                        column: x => x.lecturer_id,
                        principalTable: "Users",
                        principalColumn: "uid");
                    table.ForeignKey(
                        name: "FK__Feedback__studen__3E52440B",
                        column: x => x.student_id,
                        principalTable: "Users",
                        principalColumn: "uid");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    student_id = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<decimal>(type: "money", nullable: false),
                    method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK__Payment__student__4222D4EF",
                        column: x => x.student_id,
                        principalTable: "Users",
                        principalColumn: "uid");
                });

            migrationBuilder.CreateTable(
                name: "ChangeClassRequests",
                columns: table => new
                {
                    request_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    class_id = table.Column<int>(type: "int", nullable: true),
                    content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    is_approved = table.Column<bool>(type: "bit", nullable: false),
                    request_class_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChangeCl__18D3B90FAB62976F", x => x.request_id);
                    table.ForeignKey(
                        name: "FK__ChangeCla__class__3B75D760",
                        column: x => x.class_id,
                        principalTable: "Class",
                        principalColumn: "class_id");
                    table.ForeignKey(
                        name: "FK__ChangeCla__reque__52593CB8",
                        column: x => x.request_class_id,
                        principalTable: "Class",
                        principalColumn: "class_id");
                    table.ForeignKey(
                        name: "FK__ChangeCla__user___3C69FB99",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "uid");
                });

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "int", nullable: false),
                    class_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Enrollme__55EC4102536F4D2E", x => new { x.student_id, x.class_id });
                    table.ForeignKey(
                        name: "FK__Enrollmen__class__3E52440B",
                        column: x => x.class_id,
                        principalTable: "Class",
                        principalColumn: "class_id");
                    table.ForeignKey(
                        name: "FK__Enrollmen__stude__36B12243",
                        column: x => x.student_id,
                        principalTable: "Users",
                        principalColumn: "uid");
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    class_id = table.Column<int>(type: "int", nullable: true),
                    slot_id = table.Column<int>(type: "int", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    daily_note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.id);
                    table.ForeignKey(
                        name: "FK__Schedule__class___4316F928",
                        column: x => x.class_id,
                        principalTable: "Class",
                        principalColumn: "class_id");
                    table.ForeignKey(
                        name: "FK__Schedule__slot_i__440B1D61",
                        column: x => x.slot_id,
                        principalTable: "StudySlot",
                        principalColumn: "slot_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailableDate_slot_id",
                table: "AvailableDate",
                column: "slot_id");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeClassRequests_class_id",
                table: "ChangeClassRequests",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeClassRequests_request_class_id",
                table: "ChangeClassRequests",
                column: "request_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeClassRequests_user_id",
                table: "ChangeClassRequests",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Class_lecturer_id",
                table: "Class",
                column: "lecturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_class_id",
                table: "Enrollment",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_lecturer_id",
                table: "Feedback",
                column: "lecturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_student_id",
                table: "Feedback",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_student_id",
                table: "Payment",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_class_id",
                table: "Schedule",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_slot_id",
                table: "Schedule",
                column: "slot_id");

            migrationBuilder.CreateIndex(
                name: "IX_StudySlotDay_day_id",
                table: "StudySlotDay",
                column: "day_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_id",
                table: "Users",
                column: "role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailableDate");

            migrationBuilder.DropTable(
                name: "ChangeClassRequests");

            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "StudySlotDay");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "DateOfWeek");

            migrationBuilder.DropTable(
                name: "StudySlot");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
