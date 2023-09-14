using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontatus.Data.Migrations
{
    public partial class sexo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "Pessoa",
                type: "VARCHAR(1)",
                maxLength: 1,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Pessoa");
        }
    }
}
