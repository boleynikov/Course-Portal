using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddedSkillColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder?.AddColumn<string>(
                name: "UserSkills",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseSkills",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder?.DropColumn(
                name: "UserSkills",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CourseSkills",
                table: "Courses");
        }
    }
}
