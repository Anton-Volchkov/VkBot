using Microsoft.EntityFrameworkCore.Migrations;

namespace VkBot.Migrations
{
    public partial class EditTableWithRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "AmountChatMessages",
                table: "ChatRoles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ChatRoles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountChatMessages",
                table: "ChatRoles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ChatRoles");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}
