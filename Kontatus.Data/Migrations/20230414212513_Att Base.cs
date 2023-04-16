using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontatus.Data.Migrations
{
    public partial class AttBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultadosIN100");

            migrationBuilder.DropTable(
                name: "SolicitacoesIN100");

            migrationBuilder.DropTable(
                name: "SolicitacoesOffline");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroTelefone",
                table: "Telefone",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Pessoa",
                type: "VARCHAR(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataNascimento",
                table: "Pessoa",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Pessoa",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoEndereco",
                table: "Endereco",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Endereco",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cidade",
                table: "Endereco",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bairro",
                table: "Endereco",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumeroTelefone",
                table: "Telefone",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Pessoa",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataNascimento",
                table: "Pessoa",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Pessoa",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoEndereco",
                table: "Endereco",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Endereco",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cidade",
                table: "Endereco",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bairro",
                table: "Endereco",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ResultadosIN100",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeneficiarioID = table.Column<int>(type: "int", nullable: true),
                    Beneficio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeneficioID = table.Column<int>(type: "int", nullable: true),
                    Cpf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DDB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DIB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataAtualizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataNascimento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmprestimoBloqueado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmprestimoElegivel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Idade = table.Column<int>(type: "int", nullable: true),
                    InstituicaoFinanceiraID = table.Column<int>(type: "int", nullable: true),
                    MargemConsignavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MargemConsignavelCartao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MargemNula = table.Column<bool>(type: "bit", nullable: false),
                    MeioPagamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeioPagamentoID = table.Column<int>(type: "int", nullable: true),
                    MensagemServidor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeAgencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroAgencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroBeneficio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroContaCorrente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrigemBancoID = table.Column<int>(type: "int", nullable: true),
                    PossuiProcurador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PossuiRepresentanteLegal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QtdEmprestimosAtivosSuspensos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RMCAtivo = table.Column<int>(type: "int", nullable: true),
                    RequisicaoID = table.Column<int>(type: "int", nullable: true),
                    Situacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolicitacaoID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UFBeneficio = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosIN100", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacoesIN100",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CreditoExtrato = table.Column<bool>(type: "bit", nullable: true),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroBeneficio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolicitacaoID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusProcessamento = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacoesIN100", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SolicitacoesIN100_Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacoesOffline",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroBeneficio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacoesOffline", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SolicitacoesOffline_Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesIN100_UsuarioID",
                table: "SolicitacoesIN100",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesOffline_UsuarioID",
                table: "SolicitacoesOffline",
                column: "UsuarioID");
        }
    }
}
