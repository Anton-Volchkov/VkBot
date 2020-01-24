using Microsoft.EntityFrameworkCore.Migrations;

namespace VkBot.Migrations
{
    public partial class AddColumnRebuke : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Rebuke",
                table: "ChatRoles",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rebuke",
                table: "ChatRoles");
        }
    }
}
