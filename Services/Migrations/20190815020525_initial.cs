using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "JobId", "Name" },
                values: new object[] { 1, "Job 1" });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "JobId", "Name" },
                values: new object[] { 2, "Job 2" });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "JobId", "Name" },
                values: new object[] { 3, "Job 3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
