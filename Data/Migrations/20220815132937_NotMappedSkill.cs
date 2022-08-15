using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class NotMappedSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder?.DropTable(
                name: "Skill");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder?.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Name = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Skill_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Skill_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skill_CourseId",
                table: "Skill",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_UserId",
                table: "Skill",
                column: "UserId");
        }
    }
}
