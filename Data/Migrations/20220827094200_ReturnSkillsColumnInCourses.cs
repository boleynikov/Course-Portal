using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ReturnSkillsColumnInCourses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder?.AddColumn<string>(
                name: "CourseSkills",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder?.DropColumn(
                name: "CourseSkills",
                table: "Courses");
        }
    }
}
