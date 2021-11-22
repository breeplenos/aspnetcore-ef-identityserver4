using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class ChangeCourseIDReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignment_Course_CourseID",
                table: "CourseAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                table: "Enrollment");

            migrationBuilder.DropIndex(name: "IX_Enrollment_CourseID", table: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.Sql("UPDATE dbo.CourseAssignment SET CourseID = (SELECT ID FROM dbo.Course WHERE dbo.Course.CourseID = dbo.CourseAssignment.CourseID)");
            migrationBuilder.Sql("UPDATE dbo.Enrollment SET CourseID = (SELECT ID FROM dbo.Course WHERE dbo.Course.CourseID = dbo.Enrollment.CourseID)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "ID");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "Course",
                newName: "CourseNumber");

            migrationBuilder.CreateIndex(
                 name: "IX_Enrollment_CourseID",
                 table: "Enrollment",
                 column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignment_Course_CourseID",
                table: "CourseAssignment",
                column: "CourseID",
                principalTable: "Course",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                table: "Enrollment",
                column: "CourseID",
                principalTable: "Course",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignment_Course_CourseID",
                table: "CourseAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                table: "Enrollment");

            migrationBuilder.DropIndex(name: "IX_Enrollment_CourseID", table: "Enrollment");

            migrationBuilder.Sql("UPDATE dbo.CourseAssignment SET CourseID = (SELECT CourseNumber FROM dbo.Course WHERE dbo.Course.ID = dbo.CourseAssignment.CourseID)");
            migrationBuilder.Sql("UPDATE dbo.Enrollment SET CourseID = (SELECT CourseNumber FROM dbo.Course WHERE dbo.Course.ID = dbo.Enrollment.CourseID)");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "CourseNumber",
                table: "Course",
                newName: "CourseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "CourseID");

            migrationBuilder.DropColumn(name: "ID", table: "Course");

            migrationBuilder.CreateIndex(
                 name: "IX_Enrollment_CourseID",
                 table: "Enrollment",
                 column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignment_Course_CourseID",
                table: "CourseAssignment",
                column: "CourseID",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                table: "Enrollment",
                column: "CourseID",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
