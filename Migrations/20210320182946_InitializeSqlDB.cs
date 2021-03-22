using Microsoft.EntityFrameworkCore.Migrations;

namespace kevintrinh1227.Migrations
{
    public partial class InitializeSqlDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WarnedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    warningNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    memberId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    reason = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarnedUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarnedUsers");
        }
    }
}
