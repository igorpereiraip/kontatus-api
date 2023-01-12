using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontatus.Data.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Perfil",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Administrador = table.Column<bool>(type: "bit", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfil", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosIN100",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitacaoID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeneficiarioID = table.Column<int>(type: "int", nullable: true),
                    Cpf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroBeneficio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataNascimento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmprestimoBloqueado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmprestimoElegivel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DIB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeneficioID = table.Column<int>(type: "int", nullable: true),
                    MargemConsignavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RMCAtivo = table.Column<int>(type: "int", nullable: true),
                    UFBeneficio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeioPagamentoID = table.Column<int>(type: "int", nullable: true),
                    InstituicaoFinanceiraID = table.Column<int>(type: "int", nullable: true),
                    NomeAgencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroAgencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroContaCorrente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAtualizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MensagemServidor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DDB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequisicaoID = table.Column<int>(type: "int", nullable: true),
                    Situacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrigemBancoID = table.Column<int>(type: "int", nullable: true),
                    MargemConsignavelCartao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QtdEmprestimosAtivosSuspensos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PossuiRepresentanteLegal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PossuiProcurador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Idade = table.Column<int>(type: "int", nullable: true),
                    MeioPagamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Beneficio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MargemNula = table.Column<bool>(type: "bit", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosIN100", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Administrador = table.Column<bool>(type: "bit", nullable: false),
                    SaldoIN100 = table.Column<int>(type: "int", nullable: false),
                    SaldoExtrato = table.Column<int>(type: "int", nullable: false),
                    SaldoOffline = table.Column<int>(type: "int", nullable: true),
                    OfflineIlimitado = table.Column<bool>(type: "bit", nullable: true),
                    LimiteDiario = table.Column<int>(type: "int", nullable: true),
                    AcessosSimultaneos = table.Column<int>(type: "int", nullable: true),
                    ValidadePlano = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Arquivo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaminhoImagem = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    NomeBlob = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    NomeCompleto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataUpload = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Beneficio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tamanho = table.Column<double>(type: "float", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arquivo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Arquivo_Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoLoginID = table.Column<int>(type: "int", nullable: false),
                    Principal = table.Column<bool>(type: "bit", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Login_Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogUsuario",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Metodo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Controle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    UrlAcionada = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RegistroAfetadoID = table.Column<int>(type: "int", nullable: true),
                    RegistroNovo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistroAntigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogUsuario", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LogUsuario_Usuario_RegistroAfetadoID",
                        column: x => x.RegistroAfetadoID,
                        principalTable: "Usuario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogUsuario_Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacoesIN100",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    SolicitacaoID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroBeneficio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusProcessamento = table.Column<int>(type: "int", nullable: false),
                    CreditoExtrato = table.Column<bool>(type: "bit", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
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
                    CPF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroBeneficio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JWT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataExpiracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Token_Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arquivo_UsuarioID",
                table: "Arquivo",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Login_UsuarioID",
                table: "Login",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_LogUsuario_RegistroAfetadoID",
                table: "LogUsuario",
                column: "RegistroAfetadoID");

            migrationBuilder.CreateIndex(
                name: "IX_LogUsuario_UsuarioID",
                table: "LogUsuario",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesIN100_UsuarioID",
                table: "SolicitacoesIN100",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesOffline_UsuarioID",
                table: "SolicitacoesOffline",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Token_UsuarioID",
                table: "Token",
                column: "UsuarioID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arquivo");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "LogUsuario");

            migrationBuilder.DropTable(
                name: "Perfil");

            migrationBuilder.DropTable(
                name: "ResultadosIN100");

            migrationBuilder.DropTable(
                name: "SolicitacoesIN100");

            migrationBuilder.DropTable(
                name: "SolicitacoesOffline");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
