using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Faturamento.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotasFiscais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NumeroSequencial = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasFiscais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotaFiscalItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NotaFiscalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProdutoCodigo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaFiscalItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotaFiscalItems_NotasFiscais_NotaFiscalId",
                        column: x => x.NotaFiscalId,
                        principalTable: "NotasFiscais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotaFiscalItems_NotaFiscalId",
                table: "NotaFiscalItems",
                column: "NotaFiscalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotaFiscalItems");

            migrationBuilder.DropTable(
                name: "NotasFiscais");
        }
    }
}
