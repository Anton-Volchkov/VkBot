using Microsoft.EntityFrameworkCore.Migrations;

namespace VkBot.Data.Migrations
{
    public partial class Memories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Memory",
                table: "Memory");

            migrationBuilder.RenameTable(
                name: "Memory",
                newName: "Memories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memories",
                table: "Memories",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Memories",
                table: "Memories");

            migrationBuilder.RenameTable(
                name: "Memories",
                newName: "Memory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memory",
                table: "Memory",
                column: "Id");
        }
    }
}
