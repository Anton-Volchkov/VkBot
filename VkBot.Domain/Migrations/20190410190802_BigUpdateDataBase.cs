using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;


namespace VkBot.Domain.Migrations
{
    public partial class BigUpdateDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                                         "Commons",
                                         table => new
                                         {
                                             Id = table.Column<int>()
                                                       .Annotation("Npgsql:ValueGenerationStrategy",
                                                                   NpgsqlValueGenerationStrategy.SerialColumn),
                                             commonInfo = table.Column<string>(nullable: true)
                                         },
                                         constraints: table => { table.PrimaryKey("PK_Commons", x => x.Id); });

            migrationBuilder.CreateTable(
                                         "Memories",
                                         table => new
                                         {
                                             Id = table.Column<int>()
                                                       .Annotation("Npgsql:ValueGenerationStrategy",
                                                                   NpgsqlValueGenerationStrategy.SerialColumn),
                                             UserID = table.Column<long>(),
                                             Memory = table.Column<string>(nullable: true)
                                         },
                                         constraints: table => { table.PrimaryKey("PK_Memories", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                                       "Commons");

            migrationBuilder.DropTable(
                                       "Memories");
        }
    }
}