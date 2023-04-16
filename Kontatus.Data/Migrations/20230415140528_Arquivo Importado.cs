using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontatus.Data.Migrations
{
    public partial class ArquivoImportado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArquivoImportado",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Competencia = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    Descricao = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    StatusProcessamento = table.Column<int>(type: "int", nullable: false),
                    PessoasAdicionadas = table.Column<int>(type: "int", nullable: true),
                    PessoasAlteradas = table.Column<int>(type: "int", nullable: true),
                    EnderecosAlterados = table.Column<int>(type: "int", nullable: true),
                    EnderecosCriados = table.Column<int>(type: "int", nullable: true),
                    TelefonesCriados = table.Column<int>(type: "int", nullable: true),
                    TelefonesAlterados = table.Column<int>(type: "int", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArquivoImportado", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArquivoImportado");
        }
    }
}
