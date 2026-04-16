using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estoque.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdempotencyToEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransacoesProcessadas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChaveIdempotencia = table.Column<Guid>(type: "TEXT", nullable: false),
                    DataProcessamento = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransacoesProcessadas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransacoesProcessadas_ChaveIdempotencia",
                table: "TransacoesProcessadas",
                column: "ChaveIdempotencia",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransacoesProcessadas");
        }
    }
}
