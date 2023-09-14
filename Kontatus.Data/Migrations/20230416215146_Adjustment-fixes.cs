using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontatus.Data.Migrations
{
    public partial class Adjustmentfixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArquivoImportadoId",
                table: "Telefone",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArquivoImportadoId",
                table: "Endereco",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Telefone_ArquivoImportadoId",
                table: "Telefone",
                column: "ArquivoImportadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Endereco_ArquivoImportadoId",
                table: "Endereco",
                column: "ArquivoImportadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Endereco_ArquivoImportado_ArquivoImportadoId",
                table: "Endereco",
                column: "ArquivoImportadoId",
                principalTable: "ArquivoImportado",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Telefone_ArquivoImportado_ArquivoImportadoId",
                table: "Telefone",
                column: "ArquivoImportadoId",
                principalTable: "ArquivoImportado",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
