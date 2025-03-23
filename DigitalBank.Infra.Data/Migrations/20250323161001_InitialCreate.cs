using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBank.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CONTAS",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    documento = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    saldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    data_abertura = table.Column<DateTime>(type: "TEXT", nullable: false),
                    status = table.Column<int>(type: "INTEGER", nullable: false),
                    data_inativacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    usuario_inativacao = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTAS", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TRANSFERENCIAS",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    conta_origem_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    conta_destino_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    valor = table.Column<decimal>(type: "TEXT", nullable: false),
                    data_transferencia = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSFERENCIAS", x => x.id);
                    table.ForeignKey(
                        name: "FK_TRANSFERENCIAS_CONTAS_conta_destino_id",
                        column: x => x.conta_destino_id,
                        principalTable: "CONTAS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TRANSFERENCIAS_CONTAS_conta_origem_id",
                        column: x => x.conta_origem_id,
                        principalTable: "CONTAS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CONTAS_documento",
                table: "CONTAS",
                column: "documento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TRANSFERENCIAS_conta_destino_id",
                table: "TRANSFERENCIAS",
                column: "conta_destino_id");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSFERENCIAS_conta_origem_id",
                table: "TRANSFERENCIAS",
                column: "conta_origem_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TRANSFERENCIAS");

            migrationBuilder.DropTable(
                name: "CONTAS");
        }
    }
}
