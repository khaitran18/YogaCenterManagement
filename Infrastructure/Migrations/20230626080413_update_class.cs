using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class update_class : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Available__slot___3A81B327",
                table: "AvailableDate");

            migrationBuilder.DropForeignKey(
                name: "FK__ChangeCla__class__3B75D760",
                table: "ChangeClassRequests");

            migrationBuilder.DropForeignKey(
                name: "FK__ChangeCla__reque__52593CB8",
                table: "ChangeClassRequests");

            migrationBuilder.DropForeignKey(
                name: "FK__ChangeCla__user___3C69FB99",
                table: "ChangeClassRequests");

            migrationBuilder.DropForeignKey(
                name: "FK__Enrollmen__class__3E52440B",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK__Schedule__class___4316F928",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK__Schedule__slot_i__440B1D61",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK__StudySlot__slot___45F365D3",
                table: "StudySlotDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK__StudySlo__3FAF17102EA82550",
                table: "StudySlotDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK__StudySlo__971A01BB229BD5A6",
                table: "StudySlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Enrollme__55EC4102536F4D2E",
                table: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ChangeCl__18D3B90FAB62976F",
                table: "ChangeClassRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Availabl__7DA07AAAAFCDBABB",
                table: "AvailableDate");

            migrationBuilder.AddColumn<int>(
                name: "class_capacity",
                table: "Class",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "Class",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK__StudySlo__3FAF1710B9F5AE59",
                table: "StudySlotDay",
                columns: new[] { "slot_id", "day_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__StudySlo__971A01BB6CD92A1C",
                table: "StudySlot",
                column: "slot_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Enrollme__55EC41027C0B47FF",
                table: "Enrollment",
                columns: new[] { "student_id", "class_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__ChangeCl__18D3B90FDC679E9F",
                table: "ChangeClassRequests",
                column: "request_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Availabl__7DA07AAACB79894C",
                table: "AvailableDate",
                columns: new[] { "lecturer_id", "slot_id" });

            migrationBuilder.AddForeignKey(
                name: "FK__Available__slot___73BA3083",
                table: "AvailableDate",
                column: "slot_id",
                principalTable: "StudySlot",
                principalColumn: "slot_id");

            migrationBuilder.AddForeignKey(
                name: "FK__ChangeCla__class__74AE54BC",
                table: "ChangeClassRequests",
                column: "class_id",
                principalTable: "Class",
                principalColumn: "class_id");

            migrationBuilder.AddForeignKey(
                name: "FK__ChangeCla__reque__75A278F5",
                table: "ChangeClassRequests",
                column: "request_class_id",
                principalTable: "Class",
                principalColumn: "class_id");

            migrationBuilder.AddForeignKey(
                name: "FK__ChangeCla__user___76969D2E",
                table: "ChangeClassRequests",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "uid");

            migrationBuilder.AddForeignKey(
                name: "FK__Enrollmen__class__787EE5A0",
                table: "Enrollment",
                column: "class_id",
                principalTable: "Class",
                principalColumn: "class_id");

            migrationBuilder.AddForeignKey(
                name: "FK__Schedule__class___7D439ABD",
                table: "Schedule",
                column: "class_id",
                principalTable: "Class",
                principalColumn: "class_id");

            migrationBuilder.AddForeignKey(
                name: "FK__Schedule__slot_i__7E37BEF6",
                table: "Schedule",
                column: "slot_id",
                principalTable: "StudySlot",
                principalColumn: "slot_id");

            migrationBuilder.AddForeignKey(
                name: "FK__StudySlot__slot___00200768",
                table: "StudySlotDay",
                column: "slot_id",
                principalTable: "StudySlot",
                principalColumn: "slot_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Available__slot___73BA3083",
                table: "AvailableDate");

            migrationBuilder.DropForeignKey(
                name: "FK__ChangeCla__class__74AE54BC",
                table: "ChangeClassRequests");

            migrationBuilder.DropForeignKey(
                name: "FK__ChangeCla__reque__75A278F5",
                table: "ChangeClassRequests");

            migrationBuilder.DropForeignKey(
                name: "FK__ChangeCla__user___76969D2E",
                table: "ChangeClassRequests");

            migrationBuilder.DropForeignKey(
                name: "FK__Enrollmen__class__787EE5A0",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK__Schedule__class___7D439ABD",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK__Schedule__slot_i__7E37BEF6",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK__StudySlot__slot___00200768",
                table: "StudySlotDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK__StudySlo__3FAF1710B9F5AE59",
                table: "StudySlotDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK__StudySlo__971A01BB6CD92A1C",
                table: "StudySlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Enrollme__55EC41027C0B47FF",
                table: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ChangeCl__18D3B90FDC679E9F",
                table: "ChangeClassRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Availabl__7DA07AAACB79894C",
                table: "AvailableDate");

            migrationBuilder.DropColumn(
                name: "class_capacity",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Class");

            migrationBuilder.AddPrimaryKey(
                name: "PK__StudySlo__3FAF17102EA82550",
                table: "StudySlotDay",
                columns: new[] { "slot_id", "day_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__StudySlo__971A01BB229BD5A6",
                table: "StudySlot",
                column: "slot_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Enrollme__55EC4102536F4D2E",
                table: "Enrollment",
                columns: new[] { "student_id", "class_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__ChangeCl__18D3B90FAB62976F",
                table: "ChangeClassRequests",
                column: "request_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Availabl__7DA07AAAAFCDBABB",
                table: "AvailableDate",
                columns: new[] { "lecturer_id", "slot_id" });

            migrationBuilder.AddForeignKey(
                name: "FK__Available__slot___3A81B327",
                table: "AvailableDate",
                column: "slot_id",
                principalTable: "StudySlot",
                principalColumn: "slot_id");

            migrationBuilder.AddForeignKey(
                name: "FK__ChangeCla__class__3B75D760",
                table: "ChangeClassRequests",
                column: "class_id",
                principalTable: "Class",
                principalColumn: "class_id");

            migrationBuilder.AddForeignKey(
                name: "FK__ChangeCla__reque__52593CB8",
                table: "ChangeClassRequests",
                column: "request_class_id",
                principalTable: "Class",
                principalColumn: "class_id");

            migrationBuilder.AddForeignKey(
                name: "FK__ChangeCla__user___3C69FB99",
                table: "ChangeClassRequests",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "uid");

            migrationBuilder.AddForeignKey(
                name: "FK__Enrollmen__class__3E52440B",
                table: "Enrollment",
                column: "class_id",
                principalTable: "Class",
                principalColumn: "class_id");

            migrationBuilder.AddForeignKey(
                name: "FK__Schedule__class___4316F928",
                table: "Schedule",
                column: "class_id",
                principalTable: "Class",
                principalColumn: "class_id");

            migrationBuilder.AddForeignKey(
                name: "FK__Schedule__slot_i__440B1D61",
                table: "Schedule",
                column: "slot_id",
                principalTable: "StudySlot",
                principalColumn: "slot_id");

            migrationBuilder.AddForeignKey(
                name: "FK__StudySlot__slot___45F365D3",
                table: "StudySlotDay",
                column: "slot_id",
                principalTable: "StudySlot",
                principalColumn: "slot_id");
        }
    }
}
