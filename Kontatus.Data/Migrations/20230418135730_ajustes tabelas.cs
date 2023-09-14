using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontatus.Data.Migrations
{
    public partial class ajustestabelas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnderecosAlterados",
                table: "ArquivoImportado");

            migrationBuilder.DropColumn(
                name: "PessoasAlteradas",
                table: "ArquivoImportado");

            migrationBuilder.RenameColumn(
                name: "TelefonesAlterados",
                table: "ArquivoImportado",
                newName: "EmailsCriados");

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoEndereco",
                table: "Endereco",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(80)",
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Endereco",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Endereco",
                type: "VARCHAR(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Uf",
                table: "Endereco",
                type: "VARCHAR(2)",
                maxLength: 2,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Endereco");

            migrationBuilder.DropColumn(
                name: "Uf",
                table: "Endereco");

            migrationBuilder.RenameColumn(
                name: "EmailsCriados",
                table: "ArquivoImportado",
                newName: "TelefonesAlterados");

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoEndereco",
                table: "Endereco",
                type: "VARCHAR(80)",
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Endereco",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnderecosAlterados",
                table: "ArquivoImportado",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PessoasAlteradas",
                table: "ArquivoImportado",
                type: "int",
                nullable: true);
        }
    }
}
