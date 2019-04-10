using Microsoft.EntityFrameworkCore.Migrations;

namespace VkBot.Migrations
{
    public partial class RewiewDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "commonInfo",
                table: "Commons",
                newName: "СommonInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "СommonInfo",
                table: "Commons",
                newName: "commonInfo");
        }
    }
}
