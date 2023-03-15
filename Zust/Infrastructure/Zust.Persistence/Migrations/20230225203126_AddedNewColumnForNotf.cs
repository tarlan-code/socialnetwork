using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zust.Persistence.Migrations
{
    public partial class AddedNewColumnForNotf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReaded",
                table: "Notification",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReaded",
                table: "Notification");
        }
    }
}
