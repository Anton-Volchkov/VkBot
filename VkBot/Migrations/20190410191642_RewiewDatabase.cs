using Microsoft.EntityFrameworkCore.Migrations;

namespace VkBot.Migrations
{
    public partial class RewiewDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                                          "commonInfo",
                                          "Commons",
                                          "СommonInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                                          "СommonInfo",
                                          "Commons",
                                          "commonInfo");
        }
    }
}