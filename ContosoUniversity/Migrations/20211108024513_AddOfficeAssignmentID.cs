using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class AddOfficeAssignmentID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficeAssignment",
                table: "OfficeAssignment");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "OfficeAssignment",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficeAssignment",
                table: "OfficeAssignment",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficeAssignment",
                table: "OfficeAssignment");

            migrationBuilder.DropColumn(name: "ID", table: "OfficeAssignment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficeAssignment",
                table: "OfficeAssignment",
                column: "InstructorID");
        }
    }
}
