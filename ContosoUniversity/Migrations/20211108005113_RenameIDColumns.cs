using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class RenameIDColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnrollmentID",
                table: "Enrollment",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "DepartmentID",
                table: "Department",
                newName: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Enrollment",
                newName: "EnrollmentID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Department",
                newName: "DepartmentID");
        }
    }
}
