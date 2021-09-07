using Microsoft.EntityFrameworkCore.Migrations;

namespace CommentService.DbMigrations.Migrations
{
    public partial class AddConsumedMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumedEvents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TimeOfReceived = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumedEvents", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumedEvents");
        }
    }
}
