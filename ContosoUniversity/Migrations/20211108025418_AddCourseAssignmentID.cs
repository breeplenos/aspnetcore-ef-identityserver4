using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class AddCourseAssignmentID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseAssignment",
                table: "CourseAssignment");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "CourseAssignment",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseAssignment",
                table: "CourseAssignment",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseAssignment",
                table: "CourseAssignment");

            migrationBuilder.DropColumn(name: "ID", table: "CourseAssignment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseAssignment",
                table: "CourseAssignment",
                columns: new[] { "CourseID", "InstructorID" });
        }
    }
}
