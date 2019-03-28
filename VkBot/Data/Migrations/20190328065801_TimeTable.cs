using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace VkBot.Data.Migrations
{
    public partial class TimeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                                         "TimeTables",
                                         table => new
                                         {
                                             Id = table.Column<int>(nullable: false)
                                                       .Annotation("Npgsql:ValueGenerationStrategy",
                                                                   NpgsqlValueGenerationStrategy.SerialColumn),
                                             Timetable = table.Column<string>(nullable: true)
                                         },
                                         constraints: table => { table.PrimaryKey("PK_TimeTables", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                                       "TimeTables");
        }
    }
}